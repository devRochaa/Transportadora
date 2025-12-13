using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Data.Entities;

public class DeliveryRoute : BaseEntity
{
    public RouteStatus Status { get; set; }

    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }

    public Guid DeliveryId { get; set; }
    public Delivery Delivery { get; set; } = default!;

    public Guid DriverId { get; set; }
    public Driver Driver { get; set; } = default!;

    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = default!;

    public ICollection<DeliveryRouteStop> Stops { get; set; } = new List<DeliveryRouteStop>();
    public ICollection<VehiclePosition> Positions { get; set; } = new List<VehiclePosition>();
}
