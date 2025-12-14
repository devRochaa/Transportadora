using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportadora.API.Data;
using Transportadora.API.Data.Entities;
using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Controllers;

[ApiController]
[Route("api/routes/{routeId:guid}/stops")]
public sealed class RouteStopsController(ApplicationDbContext db) : ControllerBase
{
    // registra chegada em uma parada
    [HttpPost]
    public async Task<IActionResult> Arrive(
        Guid routeId,
        RouteStopType type,
        double latitude,
        double longitude,
        CancellationToken ct)
    {
        var route = await db.Routes
            .Include(r => r.Stops)
            .FirstOrDefaultAsync(r => r.Id == routeId, ct);

        if (route is null)
            return NotFound("Rota não encontrada");

        if (route.Status != RouteStatus.InProgress)
            return BadRequest("Rota não está em execução");

        // evita duas paradas abertas ao mesmo tempo
        var openStop = route.Stops
            .FirstOrDefault(s => s.DepartedAt == null);

        if (openStop is not null)
            return Conflict("Existe uma parada em aberto");

        var nextSequence = route.Stops.Count != 0
            ? route.Stops.Max(s => s.Sequence) + 1
            : 1;

        var stop = new DeliveryRouteStop
        {
            RouteId = routeId,
            Type = type,
            Latitude = latitude,
            Longitude = longitude,
            Sequence = nextSequence,
            ArrivedAt = DateTimeOffset.UtcNow
        };

        db.RouteStops.Add(stop);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById),
            new { routeId, stopId = stop.Id }, stop);
    }

    // registra saída da parada
    [HttpPost("{stopId:guid}/depart")]
    public async Task<IActionResult> Depart(
        Guid routeId,
        Guid stopId,
        CancellationToken ct)
    {
        var stop = await db.RouteStops
            .Include(s => s.Route)
            .FirstOrDefaultAsync(
                s => s.Id == stopId && s.RouteId == routeId,
                ct);

        if (stop is null)
            return NotFound("Parada não encontrada");

        if (stop.Route!.Status != RouteStatus.InProgress)
            return BadRequest("Rota não está em execução");

        if (stop.DepartedAt is not null)
            return BadRequest("Parada já foi finalizada");

        stop.DepartedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // consulta uma parada específica
    [HttpGet("{stopId:guid}")]
    public async Task<IActionResult> GetById(
        Guid routeId,
        Guid stopId,
        CancellationToken ct)
    {
        var stop = await db.RouteStops
            .Where(s => s.RouteId == routeId && s.Id == stopId)
            .Select(s => new
            {
                s.Id,
                s.Type,
                s.Sequence,
                s.Latitude,
                s.Longitude,
                s.ArrivedAt,
                s.DepartedAt
            })
            .FirstOrDefaultAsync(ct);

        if (stop is null)
            return NotFound();

        return Ok(stop);
    }

    // lista todas as paradas da rota
    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid routeId,
        CancellationToken ct)
    {
        var stops = await db.RouteStops
            .Where(s => s.RouteId == routeId)
            .OrderBy(s => s.Sequence)
            .Select(s => new
            {
                s.Id,
                s.Type,
                s.Sequence,
                s.ArrivedAt,
                s.DepartedAt
            })
            .ToListAsync(ct);

        return Ok(stops);
    }
}
