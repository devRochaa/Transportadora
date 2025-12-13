using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Data.Entities;

public class DeliveryRouteStop : BaseEntity
{
    public RouteStopType Type { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public DateTimeOffset? PlannedAt { get; set; }

    public DateTimeOffset? ArrivedAt { get; set; }
    public DateTimeOffset? DepartedAt { get; set; }

    public int Sequence { get; set; }

    public Guid RouteId { get; set; }
    public DeliveryRoute Route { get; set; } = default!;
}
