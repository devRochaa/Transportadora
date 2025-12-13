using Microsoft.EntityFrameworkCore;
using Transportadora.API.Data.Entities;

namespace Transportadora.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<DeliveryRoute> Routes => Set<DeliveryRoute>();
    public DbSet<DeliveryRouteStop> RouteStops => Set<DeliveryRouteStop>();
    public DbSet<VehiclePosition> VehiclePositions => Set<VehiclePosition>();
    public DbSet<DeliveryEvent> DeliveryEvents => Set<DeliveryEvent>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }
}
