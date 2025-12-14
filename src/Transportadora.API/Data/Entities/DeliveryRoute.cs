using System.Text.Json.Serialization;
using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Data.Entities;

public class DeliveryRoute : BaseEntity
{
    public RouteStatus Status { get; set; }

    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }

    public Guid DeliveryId { get; set; }
    [JsonIgnore]
    public Delivery Delivery { get; set; } = default!;

    public Guid? DriverId { get; set; }
    [JsonIgnore]
    public Driver? Driver { get; set; }

    public Guid? VehicleId { get; set; }
    [JsonIgnore]
    public Vehicle? Vehicle { get; set; }

    public ICollection<DeliveryRouteStop> Stops { get; set; } = new List<DeliveryRouteStop>();
    public ICollection<VehiclePosition> Positions { get; set; } = new List<VehiclePosition>();
}
