using Transportadora.Domain.Abstractions;

namespace Transportadora.Domain.Entities;

public sealed class AddressEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ZipCode { get; set; }
    public required string Country { get; set; }
    public required string State { get; set; }
    public required string City { get; set; }
    public required string Neighborhood { get; set; }
    public required string Street { get; set; } 
    public required string Number { get; set; }
    public string? Complement { get; set; }

    public Guid? ClientId { get; set; }
    public PersonEntity? Client { get; set; } 
    public Guid? SupplierId { get; set; }
    public SupplierEntity? Supplier { get; set; }
}
