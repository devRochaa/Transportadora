using System.Globalization;
using System.IO;
using System.Text.Json;
using Transportadora.Infrasctructure.Providers.CepProvider;

namespace Transportadora.Infrasctructure.Providers.Geoapify;

public static class GeoClient
{
    private static readonly string BaseUrl = "https://api.geoapify.com/v1/";

    public static readonly HttpClient httpClient = new HttpClient()
    {
        BaseAddress = new Uri(BaseUrl)
    };

    public async static Task<GeoCordinatesModel?> GetCoordinatesAsync(GetCepModel address, string apiKey)
    {
        string street = Uri.EscapeDataString(address.Logradouro);
        string cidade = Uri.EscapeDataString(address.Cidade);
        string estado = Uri.EscapeDataString(address.Uf);
        string postCode = Uri.EscapeDataString(address.Cep);

        HttpResponseMessage response = await httpClient.GetAsync($"geocode/search?country=BR&state={estado}&street={street}&city={cidade}&postcode={postCode}&apiKey={apiKey}");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<GeoapifyGeocodeResponse>(responseBody);

        if (json == null || json.Features.Length == 0)
            return null;

        var firstFeature = json.Features[0];
        return new GeoCordinatesModel
        {
            Latitude = firstFeature.Geometry.Coordinates[1],
            Longitude = firstFeature.Geometry.Coordinates[0]
        };
    }

    public static async Task<GeoDistanceModel?> CalculateDistanceAsync(GeoCordinatesModel origin,
                                                                       GeoCordinatesModel destiny,
                                                                       string apiKey)
    {
        var originLat = origin.Latitude.ToString(CultureInfo.InvariantCulture);
        var originLon = origin.Longitude.ToString(CultureInfo.InvariantCulture);
        var destinyLat = destiny.Latitude.ToString(CultureInfo.InvariantCulture);
        var destinyLon = destiny.Longitude.ToString(CultureInfo.InvariantCulture);

        string url = $"routing" +
            $"?waypoints={originLat},{originLon}|{destinyLat},{destinyLon}" +
            $"&mode=medium_truck" +
            $"&apiKey={apiKey}";

        HttpResponseMessage response = await httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<GeoapifyRouteResponse>(responseBody);

        if (json == null || json.Features.Count == 0)
            return null;

        var firstFeature = json.Features[0];
        return new GeoDistanceModel
        {
            Distance = firstFeature.Properties.Distance,
            DistanceUnits = firstFeature.Properties.DistanceUnits ?? string.Empty
        };
    }
}
