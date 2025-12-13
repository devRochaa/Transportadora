namespace Transportadora.API.Data.Entities;

public class Vehicle : BaseEntity
{
    public string Plate { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
