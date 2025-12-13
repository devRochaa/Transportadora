using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Data.Entities;

public class Delivery : BaseEntity
{
    public string OrderId { get; set; } = string.Empty;
    public DeliveryStatus Status { get; set; }
    public DateTimeOffset EstimatedStartAt { get; set; }
    public DateTimeOffset EstimatedEndAt { get; set; }
    public DateTimeOffset? ActualStartAt { get; set; }
    public DateTimeOffset? ActualEndAt { get; set; }

    public ICollection<DeliveryRoute> Routes { get; set; } = new List<DeliveryRoute>();
    public ICollection<DeliveryEvent> Events { get; set; } = new List<DeliveryEvent>();
}
