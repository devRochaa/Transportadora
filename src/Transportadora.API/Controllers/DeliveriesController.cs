using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportadora.API.Data;
using Transportadora.API.Data.Entities;
using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Controllers;

[ApiController]
[Route("api/deliveries")]
public sealed class DeliveriesController(ApplicationDbContext db) : ControllerBase
{
    // cria apenas a delivery (pedido já existe fora)
    [HttpPost]
    public async Task<IActionResult> Create(string orderId, CancellationToken ct)
    {
        var delivery = new Delivery
        {
            OrderId = orderId,
            Status = DeliveryStatus.Pending,
            EstimatedStartAt = DateTime.UtcNow, // TODO: calcular baseado em rota
            EstimatedEndAt = DateTime.UtcNow.AddDays(7), // TODO: calcular baseado em rota
        };

        db.Deliveries.Add(delivery);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = delivery.Id }, delivery);
    }

    // finaliza a entrega (manualmente)
    [HttpPost("{id:guid}/finish")]
    public async Task<IActionResult> Finish(Guid id, CancellationToken ct)
    {
        var delivery = await db.Deliveries
            .Include(d => d.Routes)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        if (delivery is null)
            return NotFound();

        delivery.Status = DeliveryStatus.Delivered;
        delivery.ActualEndAt = DateTimeOffset.UtcNow;

        foreach (var route in delivery.Routes.Where(r => r.Status == RouteStatus.InProgress))
        {
            route.Status = RouteStatus.Finished;
            route.FinishedAt = DateTimeOffset.UtcNow;
        }

        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    // estado atual da entrega
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var delivery = await db.Deliveries
            .Where(d => d.Id == id)
            .Select(d => new
            {
                d.Id,
                d.Status,

                ActiveRoute = d.Routes
                    .Where(r => r.Status == RouteStatus.InProgress)
                    .Select(r => new
                    {
                        r.Id,
                        r.DriverId,
                        r.VehicleId,
                        r.StartedAt
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(ct);

        if (delivery is null)
            return NotFound();

        return Ok(delivery);
    }

    // timeline completa da entrega (todas as rotas)
    [HttpGet("{id:guid}/timeline")]
    public async Task<IActionResult> Timeline(Guid id, CancellationToken ct)
    {
        var timeline = await db.Routes
            .Where(r => r.DeliveryId == id)
            .SelectMany(r => r.Stops)
            .OrderBy(s => s.ArrivedAt)
            .Select(s => new
            {
                s.RouteId,
                s.Type,
                s.PlannedAt,
                s.ArrivedAt,
                s.DepartedAt,
                s.Sequence
            })
            .ToListAsync(ct);

        return Ok(timeline);
    }
}
