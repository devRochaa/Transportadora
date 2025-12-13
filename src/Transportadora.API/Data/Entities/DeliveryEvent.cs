using Transportadora.API.Shared.Enums;

namespace Transportadora.API.Data.Entities;

public class DeliveryEvent : BaseEntity
{
    public DeliveryEventType Type { get; set; }
    public string Description { get; set; } = string.Empty;

    public Guid DeliveryId { get; set; }
    public Delivery Delivery { get; set; } = default!;
}