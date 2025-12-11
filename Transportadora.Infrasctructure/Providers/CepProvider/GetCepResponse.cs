using System.Text.Json.Serialization;

namespace Transportadora.Infrasctructure.Providers.CepProvider;

public class GetCepResponse
{
    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("logradouro")]
    public string Logradouro { get; set; } = string.Empty;

    [JsonPropertyName("complemento")]
    public string Complemento { get; set; } = string.Empty;

    [JsonPropertyName("bairro")]
    public string Bairro { get; set; } = string.Empty;

    [JsonPropertyName("localidade")]
    public string Localidade { get; set; }  = string.Empty;

    [JsonPropertyName("uf")]
    public string Uf { get; set; } = string.Empty;

    [JsonPropertyName("erro")]
    public bool Erro { get; set; } = false;
}
//{
//      "cep": "01001-000",
//      "logradouro": "Praça da Sé",
//      "complemento": "lado ímpar",
//      "unidade": "",
//      "bairro": "Sé",
//      "localidade": "São Paulo",
//      "uf": "SP",
//      "estado": "São Paulo",
//      "regiao": "Sudeste",
//      "ibge": "3550308",
//      "gia": "1004",
//      "ddd": "11",
//      "siafi": "7107"
//    }
        