namespace Transportadora.Infrasctructure.Providers.Geoapify;

using System.Text.Json.Serialization;

public class GeoapifyRouteResponse
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("features")]
    public List<RouteFeature> Features { get; set; } = new();

    // propriedades gerais da coleção (não da feature individual)
    [JsonPropertyName("properties")]
    public RouteCollectionProperties? Properties { get; set; }
}

public class RouteCollectionProperties
{
    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("units")]
    public string? Units { get; set; }

    [JsonPropertyName("waypoints")]
    public List<RouteWaypointSummary> Waypoints { get; set; } = new();
}

public class RouteWaypointSummary
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }
}

public class RouteFeature
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("properties")]
    public RouteFeatureProperties Properties { get; set; } = new();

    [JsonPropertyName("geometry")]
    public RouteGeometry Geometry { get; set; } = new();
}

public class RouteFeatureProperties
{
    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("units")]
    public string? Units { get; set; }

    [JsonPropertyName("distance")]
    public int Distance { get; set; }

    [JsonPropertyName("distance_units")]
    public string? DistanceUnits { get; set; }

    [JsonPropertyName("time")]
    public double Time { get; set; }

    // waypoints dentro da feature (location + original_index)
    [JsonPropertyName("waypoints")]
    public List<RouteWaypoint> Waypoints { get; set; } = new();

    [JsonPropertyName("legs")]
    public List<RouteLeg> Legs { get; set; } = new();
}

public class RouteWaypoint
{
    // [lon, lat]
    [JsonPropertyName("location")]
    public double[] Location { get; set; } = Array.Empty<double>();

    [JsonPropertyName("original_index")]
    public int OriginalIndex { get; set; }
}

public class RouteLeg
{
    [JsonPropertyName("distance")]
    public double Distance { get; set; }

    [JsonPropertyName("time")]
    public double Time { get; set; }

    [JsonPropertyName("steps")]
    public List<RouteStep> Steps { get; set; } = new();
}

public class RouteStep
{
    [JsonPropertyName("from_index")]
    public int FromIndex { get; set; }

    [JsonPropertyName("to_index")]
    public int ToIndex { get; set; }

    [JsonPropertyName("distance")]
    public double Distance { get; set; }

    [JsonPropertyName("time")]
    public double Time { get; set; }

    [JsonPropertyName("instruction")]
    public RouteInstruction Instruction { get; set; } = new();
}

public class RouteInstruction
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class RouteGeometry
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    // MultiLineString → array de linhas → cada linha é array de [lon, lat]
    [JsonPropertyName("coordinates")]
    public double[][][] Coordinates { get; set; } = Array.Empty<double[][]>();
}

