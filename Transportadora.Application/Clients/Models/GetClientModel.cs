using System.Text.Json.Serialization;

namespace Transportadora.Application.Clients.Models;

public class GetClientModel
{

    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("fullname")]
    public string Fullname { get; set; }

    [JsonPropertyName("nationalDocument")]
    public string NationalDocument { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("birthDate")]
    public DateOnly BirthDate { get; set; }
}
