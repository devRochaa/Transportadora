using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportadora.API.Data;
using Transportadora.API.Data.Entities;
using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Controllers;

[ApiController]
[Route("api/drivers")]
public sealed class DriversController(ApplicationDbContext db) : ControllerBase
{
    // criar motorista
    [HttpPost]
    public async Task<IActionResult> Create(
        string name,
        string document,
        CancellationToken ct)
    {
        var exists = await db.Drivers
            .AnyAsync(d => d.Document == document, ct);

        if (exists)
            return BadRequest("Motorista já cadastrado");

        var driver = new Driver
        {
            Name = name,
            Document = document,
            IsActive = true,
            IsAvailable = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        db.Drivers.Add(driver);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = driver.Id }, driver);
    }

    // listar motoristas
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var drivers = await db.Drivers
            .OrderBy(d => d.Name)
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.Document,
                d.IsActive,
                d.IsAvailable
            })
            .ToListAsync(ct);

        return Ok(drivers);
    }

    // buscar por id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var driver = await db.Drivers
            .Where(d => d.Id == id)
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.Document,
                d.IsActive,
                d.IsAvailable
            })
            .FirstOrDefaultAsync(ct);

        if (driver is null)
            return NotFound();

        return Ok(driver);
    }

    // ativar / desativar
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        bool isActive,
        CancellationToken ct)
    {
        var driver = await db.Drivers.FindAsync([id], ct);
        if (driver is null)
            return NotFound();

        driver.IsActive = isActive;

        if (!isActive)
            driver.IsAvailable = false;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // marcar disponibilidade manual (folga, pausa, etc)
    [HttpPut("{id:guid}/availability")]
    public async Task<IActionResult> ChangeAvailability(
        Guid id,
        bool isAvailable,
        CancellationToken ct)
    {
        var driver = await db.Drivers.FindAsync([id], ct);
        if (driver is null)
            return NotFound();

        if (!driver.IsActive)
            return BadRequest("Motorista inativo não pode ficar disponível");

        driver.IsAvailable = isAvailable;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    // associar motorista a uma rota
    [HttpPost("{driverId:guid}/routes/{routeId:guid}")]
    public async Task<IActionResult> AssignToRoute(
        Guid driverId,
        Guid routeId,
        CancellationToken ct)
    {
        var route = await db.Routes
            .Include(r => r.Delivery)
            .FirstOrDefaultAsync(r => r.Id == routeId, ct);

        if (route is null)
            return NotFound("Rota não encontrada");

        if (route.DriverId == driverId)
            return NoContent(); // mesmo motorista já associado

        if (route.Status != RouteStatus.Planned)
            return BadRequest("Rota já iniciada");

        var driver = await db.Drivers.FindAsync([driverId], ct);
        if (driver is null)
            return NotFound("Motorista não encontrado");

        if (!driver.IsActive || !driver.IsAvailable)
            return BadRequest("Motorista indisponível");

        route.DriverId = driverId;
        driver.IsAvailable = false;

        // como está associando motorista, já pode marcar a entrega como pronta para sair se houver veículo
        if (route.VehicleId.HasValue)
        {
            route.Delivery.Status = DeliveryStatus.ReadyToGo;
        }

        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    // remover motorista da rota
    [HttpDelete("{driverId:guid}/routes/{routeId:guid}")]
    public async Task<IActionResult> UnassignFromRoute(
        Guid driverId,
        Guid routeId,
        CancellationToken ct)
    {
        var route = await db.Routes
            .Include(r => r.Delivery)
            .FirstOrDefaultAsync(
                r => r.Id == routeId && r.DriverId == driverId,
                ct);

        if (route is null)
            return NotFound();

        if (route.Status != RouteStatus.Planned)
            return BadRequest("Não é possível remover motorista após o início da rota");

        route.DriverId = null;

        var driver = await db.Drivers.FindAsync([driverId], ct);
        if (driver is not null)
        {
            driver.IsAvailable = true;
        }

        // como está removendo o motorista, volta o status da entrega para pendente se estava pronta para sair
        if (route.Delivery.Status == DeliveryStatus.ReadyToGo)
        {
            route.Delivery.Status = DeliveryStatus.Pending;
        }

        await db.SaveChangesAsync(ct);

        return NoContent();
    }
}
