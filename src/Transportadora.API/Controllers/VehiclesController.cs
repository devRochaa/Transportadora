using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportadora.API.Data;
using Transportadora.API.Data.Entities;
using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Controllers;

[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController(ApplicationDbContext db) : ControllerBase
{
    // criar veículo
    [HttpPost]
    public async Task<IActionResult> Create(
        string plate,
        string type,
        CancellationToken ct)
    {
        var exists = await db.Vehicles
            .AnyAsync(v => v.Plate == plate, ct);

        if (exists)
            return BadRequest("Veículo já cadastrado");

        var vehicle = new Vehicle
        {
            Plate = plate,
            Type = type,
            IsActive = true,
            IsAvailable = true
        };

        db.Vehicles.Add(vehicle);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
    }

    // listar veículos
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var vehicles = await db.Vehicles
            .OrderBy(v => v.Plate)
            .Select(v => new
            {
                v.Id,
                v.Plate,
                v.Type,
                v.IsActive,
                v.IsAvailable
            })
            .ToListAsync(ct);

        return Ok(vehicles);
    }

    // buscar por id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var vehicle = await db.Vehicles
            .Where(v => v.Id == id)
            .Select(v => new
            {
                v.Id,
                v.Plate,
                v.Type,
                v.IsActive,
                v.IsAvailable
            })
            .FirstOrDefaultAsync(ct);

        if (vehicle is null)
            return NotFound();

        return Ok(vehicle);
    }

    // ativar / desativar
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        bool isActive,
        CancellationToken ct)
    {
        var vehicle = await db.Vehicles.FindAsync([id], ct);
        if (vehicle is null)
            return NotFound();

        vehicle.IsActive = isActive;

        if (!isActive)
            vehicle.IsAvailable = false;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // disponibilidade manual
    [HttpPut("{id:guid}/availability")]
    public async Task<IActionResult> ChangeAvailability(
        Guid id,
        bool isAvailable,
        CancellationToken ct)
    {
        var vehicle = await db.Vehicles.FindAsync([id], ct);
        if (vehicle is null)
            return NotFound();

        if (!vehicle.IsActive)
            return BadRequest("Veículo inativo não pode ficar disponível");

        vehicle.IsAvailable = isAvailable;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    // associar veículo à rota
    [HttpPost("{vehicleId:guid}/routes/{routeId:guid}")]
    public async Task<IActionResult> AssignToRoute(
        Guid vehicleId,
        Guid routeId,
        CancellationToken ct)
    {
        var route = await db.Routes
            .Include(r => r.Delivery)
            .FirstOrDefaultAsync(r => r.Id == routeId, ct);

        if (route is null)
            return NotFound("Rota não encontrada");

        if (route.VehicleId == vehicleId)
            return NoContent(); // mesmo veículo já associado

        if (route.Status != RouteStatus.Planned)
            return BadRequest("Rota já iniciada");

        var vehicle = await db.Vehicles.FindAsync([vehicleId], ct);
        if (vehicle is null)
            return NotFound("Veículo não encontrado");

        if (!vehicle.IsActive || !vehicle.IsAvailable)
            return BadRequest("Veículo indisponível");

        route.VehicleId = vehicleId;
        vehicle.IsAvailable = false;

        // como está associando veículo, já pode marcar a rota como pronta para sair se houver motorista
        if (route.DriverId.HasValue)
        {
            route.Delivery.Status = DeliveryStatus.ReadyToGo;
        }

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // remover veículo da rota
    [HttpDelete("{vehicleId:guid}/routes/{routeId:guid}")]
    public async Task<IActionResult> UnassignFromRoute(
        Guid vehicleId,
        Guid routeId,
        CancellationToken ct)
    {
        var route = await db.Routes
            .Include(r => r.Delivery)
            .FirstOrDefaultAsync(
                r => r.Id == routeId && r.VehicleId == vehicleId,
                ct);

        if (route is null)
            return NotFound();

        if (route.Status != RouteStatus.Planned)
            return BadRequest("Não é possível remover veículo após início da rota");

        route.VehicleId = null;

        var vehicle = await db.Vehicles.FindAsync([vehicleId], ct);
        vehicle?.IsAvailable = true;

        // como está removendo o veículo, volta o status da entrega para pendente se estava pronta para sair
        if (route.Delivery.Status == DeliveryStatus.ReadyToGo)
        {
            route.Delivery.Status = DeliveryStatus.Pending;
        }

        await db.SaveChangesAsync(ct);

        return NoContent();
    }
}
