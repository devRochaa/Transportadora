using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Enums;

namespace Transportadora.Domain.Entities;

public sealed class OrderEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? MaxDeliveryDate { get; set; }
    public required int TotalAmount { get; set; }
    public WeightCategory WeightCategory { get; set; }
    public decimal Freight { get; set; }
    public decimal Distance { get; set; } //KM

    public required Guid DestinataryId { get; set; }
    public required PersonEntity Destinatary { get; set; }

    public Guid OriginId { get; set; }
    public AddressEntity? OriginAddress { get; set; }

    public Guid DestinyId { get; set; }
    public AddressEntity? DestinyAddress { get; set; }
}
