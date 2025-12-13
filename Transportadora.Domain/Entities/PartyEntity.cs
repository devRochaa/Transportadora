using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Enums;

namespace Transportadora.Domain.Entities;

public sealed class PartyEntity : IEntity, IDateTracked, ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public string Name { get; set; } = string.Empty;
    public string NationalDocument { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public PartyType Type { get; set; }

}
