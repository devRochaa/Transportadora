using Transportadora.Domain.Abstractions;

namespace Transportadora.Domain.Entities;

public sealed class CarrierEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? CompanyName { get; set; }
    public required string NationalDocument { get; set; }
    public string? ContactPhone { get; set; }
}
