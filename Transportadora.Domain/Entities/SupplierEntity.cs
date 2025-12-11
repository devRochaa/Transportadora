using Transportadora.Domain.Abstractions;

namespace Transportadora.Domain.Entities;

public sealed class SupplierEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string CompanyName { get; set; }
    public required string NationalDocument { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
}
