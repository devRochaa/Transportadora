using System.Text.Json.Serialization;

namespace Transportadora.Infrasctructure.Providers.Geoapify;

public class GeoCordinatesModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class GeoDistanceModel
{
    [JsonPropertyName("distance")]
    public required double Distance { get; set; }

    [JsonPropertyName("distance_units")]
    public required string DistanceUnits { get; set; }
}

public class GeoapifyGeocodeResponse
{
    [JsonPropertyName("features")]
    public Feature[] Features { get; set; } = Array.Empty<Feature>();
}

public class Feature
{
    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; } = default!;

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; } = default!;
}

public class Geometry
{
    [JsonPropertyName("coordinates")]
    public double[] Coordinates { get; set; } = default!;
}

public class Properties
{
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("distance")]
    public int? Distance { get; set; }

    [JsonPropertyName("distance_units")]
    public string? DistanceUnits { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("housenumber")]
    public string? Housenumber { get; set; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; set; }

    [JsonPropertyName("formatted")]
    public string? Formatted { get; set; }
}
