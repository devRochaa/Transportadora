using Transportadora.Domain.Abstractions;

namespace Transportadora.Domain.Entities;

public sealed class CarrierEntity : IEntity, IDateTracked, ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public required string Name { get; set; }
    public string? CompanyName { get; set; }
    public required string NationalDocument { get; set; }
    public string? ContactPhone { get; set; }
}
