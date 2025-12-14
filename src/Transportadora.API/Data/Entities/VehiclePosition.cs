namespace Transportadora.API.Data.Entities;

public class VehiclePosition : BaseEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    /// <summary>
    /// Velocidade em km/h
    /// </summary>
    public double Speed { get; set; }
    /// <summary>
    /// Direção em graus (0-360)
    /// </summary>
    public double Heading { get; set; }

    public DateTimeOffset CapturedAt { get; set; }

    public Guid RouteId { get; set; }
    public DeliveryRoute Route { get; set; } = default!;
}
