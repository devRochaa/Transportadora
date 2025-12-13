namespace Transportadora.API.Data.Entities;

public class VehiclePosition : BaseEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public double Speed { get; set; }
    public double Heading { get; set; }

    public DateTimeOffset CapturedAt { get; set; }

    public Guid RouteId { get; set; }
    public DeliveryRoute Route { get; set; } = default!;
}
