namespace Transportadora.API.Data.Entities;

public class Driver : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }

    public ICollection<DeliveryRoute> Routes { get; set; } = new List<DeliveryRoute>();
}
