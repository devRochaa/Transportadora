using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Transportadora.API.Data;
using Transportadora.API.Data.Entities;
using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Controllers;

[ApiController]
[Route("api/route-tracking")]
public sealed class RouteTrackingController(ApplicationDbContext db) : ControllerBase
{
    public record VehiclePositionArgs(
        [property: JsonPropertyName("latitude")] double Latitude,
        [property: JsonPropertyName("longitude")] double Longitude,
        [property: JsonPropertyName("speed")] double Speed,
        [property: JsonPropertyName("heading")] double Heading);
    // ingestão de telemetria
    [HttpPost("routes/{routeId:guid}/positions")]
    public async Task<IActionResult> Register(
        [FromRoute] Guid routeId,
        [FromBody] VehiclePositionArgs args,
        CancellationToken ct)
    {
        // valida rota (mínimo necessário)
        var routeStatus = await db.Routes
            .Where(r => r.Id == routeId)
            .Select(r => (RouteStatus?)r.Status)
            .FirstOrDefaultAsync(ct);

        if (routeStatus is null || routeStatus == RouteStatus.Planned)
            return NotFound("Rota não encontrada ou ainda não iniciada");

        if (routeStatus == RouteStatus.Finished || routeStatus == RouteStatus.Cancelled)
            return BadRequest("Rota já foi finalizada ou cancelada");

        var position = new VehiclePosition
        {
            RouteId = routeId,
            Latitude = args.Latitude,
            Longitude = args.Longitude,
            Speed = args.Speed,
            Heading = args.Heading,
            CapturedAt = DateTimeOffset.UtcNow
        };

        db.VehiclePositions.Add(position);
        await db.SaveChangesAsync(ct);

        return Accepted();
    }

    // última posição de uma rota
    [HttpGet("routes/{routeId:guid}/last")]
    public async Task<IActionResult> GetLastByRoute(
        Guid routeId,
        CancellationToken ct)
    {
        var position = await db.VehiclePositions
            .Where(p => p.RouteId == routeId)
            .OrderByDescending(p => p.CapturedAt)
            .Select(p => new
            {
                p.Latitude,
                p.Longitude,
                p.Speed,
                p.Heading,
                p.CapturedAt
            })
            .FirstOrDefaultAsync(ct);

        if (position is null)
            return NotFound();

        return Ok(position);
    }

    // última posição da delivery (rota ativa)
    [HttpGet("deliveries/{deliveryId:guid}/last")]
    public async Task<IActionResult> GetLastByDelivery(
        Guid deliveryId,
        CancellationToken ct)
    {
        var position = await db.Routes
            .Where(r =>
                r.DeliveryId == deliveryId &&
                r.Status == RouteStatus.InProgress)
            .SelectMany(r => r.Positions)
            .OrderByDescending(p => p.CapturedAt)
            .Select(p => new
            {
                p.Latitude,
                p.Longitude,
                p.Speed,
                p.Heading,
                p.CapturedAt
            })
            .FirstOrDefaultAsync(ct);

        if (position is null)
            return NotFound();

        return Ok(position);
    }
}
