using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Enums;

namespace Transportadora.Domain.Entities;

public sealed class OrderTrackEntity : IEntity, IDateTracked, ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public required DateTime Date { get; set; }
    public required DeliveryStatus Status { get; set; }
    public required string OriginLocation { get; set; }
    public required string DestinyLocation { get; set; }
    public string? Description { get; set; }
    public bool HasArrived { get; set; } = false;
    public decimal Distance { get; set; } //KM


    public Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }
    public Guid CarrierId { get; set; }
    public CarrierEntity? Carrier { get; set; }

}
