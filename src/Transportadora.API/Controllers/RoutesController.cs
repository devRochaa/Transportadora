using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportadora.API.Data;
using Transportadora.API.Data.Entities;
using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Controllers;

[ApiController]
[Route("api/routes")]
public sealed class RoutesController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet("{routeId}/timeline")]
    public async Task<IActionResult> Get([FromRoute] Guid routeId, CancellationToken cancellationToken)
    {
        var timeline = await db.VehiclePositions
            .Where(vp => vp.RouteId == routeId)
            .OrderBy(vp => vp.CapturedAt)
            .Select(vp => new
            {
                vp.Latitude,
                vp.Longitude,
                vp.CapturedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(timeline);
    }

    // cria uma nova rota (troca de motorista, veículo ou filial)
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid deliveryId,
        Guid? driverId,
        Guid? vehicleId,
        CancellationToken ct)
    {
        var delivery = await db.Deliveries
            .Include(d => d.Routes)
            .FirstOrDefaultAsync(d => d.Id == deliveryId, ct);

        if (delivery is null)
            return NotFound("Delivery não encontrado");

        if (delivery.Routes.Any(r => r.Status == RouteStatus.InProgress))
            return BadRequest("Existe uma rota em execução para este delivery");

        Driver? driver = null;
        Vehicle? vehicle = null;

        if (driverId.HasValue)
        {
            driver = await db.Drivers
                .Include(d => d.Routes)
                .FirstOrDefaultAsync(d => d.Id == driverId && d.IsActive, ct);

            if (driver is null)
                return BadRequest("Motorista inexistente ou inativo");

            if (!driver.IsAvailable)
            {
                var isSameDelivery = driver.Routes.Any(r =>
                    r.DeliveryId == deliveryId &&
                    r.Status != RouteStatus.InProgress);

                if (!isSameDelivery)
                    return BadRequest("Motorista indisponível no momento");
            }
        }

        if (vehicleId.HasValue)
        {
            vehicle = await db.Vehicles
                .Include(v => v.Routes)
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.IsActive, ct);

            if (vehicle is null)
                return BadRequest("Veículo inexistente ou inativo");

            if (!vehicle.IsAvailable)
            {
                var isSameDelivery = vehicle.Routes.Any(r =>
                    r.DeliveryId == deliveryId &&
                    r.Status != RouteStatus.InProgress);

                if (!isSameDelivery)
                    return BadRequest("Veículo indisponível no momento");
            }
        }

        var route = new DeliveryRoute
        {
            DeliveryId = deliveryId,
            DriverId = driver?.Id,
            VehicleId = vehicle?.Id,
            Status = RouteStatus.Planned,
        };

        db.Routes.Add(route);

        if (driver is not null)
        {
            driver.IsAvailable = false;
        }

        if (vehicle is not null)
        {
            vehicle.IsAvailable = false;
        }

        if (delivery.Status == DeliveryStatus.Pending
            && driver is not null
            && vehicle is not null)
        {
            delivery.Status = DeliveryStatus.ReadyToGo;
        }

        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = route.Id }, route);
    }

    // inicia execução da rota
    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id, CancellationToken ct)
    {
        var route = await db.Routes
            .Include(r => r.Delivery)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (route is null)
            return NotFound();

        if (route.Status == RouteStatus.InProgress)
            return BadRequest("Rota já está em execução");

        if (route.Status != RouteStatus.Planned
            || route.Delivery.Status != DeliveryStatus.ReadyToGo)
            return BadRequest("Rota não pode ser iniciada neste momento pois não está planejada ou a entrega não está pronta para partir");

        if (!route.DriverId.HasValue)
            return BadRequest("Rota não possui motorista atribuído");

        if (!route.VehicleId.HasValue)
            return BadRequest("Rota não possui veículo atribuído");

        // garante unicidade de rota ativa
        var hasActiveRoute = await db.Routes
            .AnyAsync(r =>
                r.DeliveryId == route.DeliveryId &&
                r.Status == RouteStatus.InProgress,
                ct);

        if (hasActiveRoute)
            return Conflict("Já existe uma rota em andamento para esta entrega");

        route.Status = RouteStatus.InProgress;
        route.StartedAt = DateTimeOffset.UtcNow;

        // inicia o status da entrega se for a primeira rota a ser iniciada
        if (!db.Routes.Any(r =>
                r.DeliveryId == route.DeliveryId &&
                r.Status == RouteStatus.Finished))
        {
            route.Delivery.Status = DeliveryStatus.InTransit;
            route.Delivery.ActualStartAt = DateTimeOffset.UtcNow;
        }

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/finish")]
    public async Task<IActionResult> Finish(Guid id, CancellationToken ct)
    {
        var route = await db.Routes
            .Include(r => r.Delivery)
            .Include(r => r.Driver)
            .Include(r => r.Vehicle)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (route is null)
            return NotFound();

        if (route.Status != RouteStatus.InProgress)
            return BadRequest("Rota não está em execução");

        route.Status = RouteStatus.Finished;
        route.FinishedAt = DateTimeOffset.UtcNow;

        // libera motorista
        if (route.Driver is not null)
        {
            route.Driver.IsAvailable = true;
        }

        // libera veículo
        if (route.Vehicle is not null)
        {
            route.Vehicle.IsAvailable = true;
        }

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // consulta da rota
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var route = await db.Routes
            .Where(r => r.Id == id)
            .Select(r => new
            {
                r.Id,
                r.Status,
                r.DriverId,
                r.VehicleId,
                r.StartedAt,
                r.FinishedAt,

                Stops = r.Stops
                    .OrderBy(s => s.Sequence)
                    .Select(s => new
                    {
                        s.Type,
                        s.ArrivedAt,
                        s.DepartedAt
                    })
            })
            .FirstOrDefaultAsync(ct);

        if (route is null)
            return NotFound();

        return Ok(route);
    }

    // TODO: cancelamento: se uma rota Planned for cancelada:
    // - libera motorista e veículo
    // - se for a única rota: volta o status da entrega para Pending

    // TODO: paradas durante a rota (RouteStop)
}
