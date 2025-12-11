using Transportadora.Domain.Enums;

namespace Transportadora.Application.Order.Requests;

public sealed class CreateOrderRequest
{
    public Guid DestinaryId { get; set; }
    public required int TotalAmount { get; set; }
    public WeightCategory WeightCategory { get; set; }

    public required string DestinyZipCode { get; set; }

    public Guid OriginId { get; set; }
}
