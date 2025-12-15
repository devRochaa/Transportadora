namespace Transportadora.API.DTOs.Models;

public class GetRouteFromTimeAgoModel
{
    public required RouteModel RouteBefore { get; set; }
    public required RouteModel RouteNow { get; set; }
}

public class RouteModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Speed { get; set; }
    public double Heading { get; set; }
    public DateTimeOffset CapturedAt { get; set; }
}