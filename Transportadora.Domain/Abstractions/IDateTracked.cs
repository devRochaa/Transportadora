namespace Transportadora.Domain.Abstractions;

internal interface IDateTracked
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset LastUpdatedAt { get; set; }
}
