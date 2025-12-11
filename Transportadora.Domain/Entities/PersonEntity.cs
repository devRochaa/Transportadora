using Transportadora.Domain.Abstractions;

namespace Transportadora.Domain.Entities;

public sealed class PersonEntity : IEntity{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Fullname { get; set; }
    public required string NationalDocument { get; set; }
    public required string Phone { get; set; }
    public DateOnly BirthDate { get; set; }
}
