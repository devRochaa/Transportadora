using System.Text.Json;

namespace Transportadora.Infrasctructure.Providers.CepProvider;

public static partial class CepClient
{
    private static readonly string BaseUrl = "https://viacep.com.br/ws/";

    public static readonly HttpClient httpClient = new HttpClient() { 
        BaseAddress = new Uri(BaseUrl) 
    };


    public static async Task<GetCepModel?> GetCepAsync(string cep)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{cep}/json/");
        response.EnsureSuccessStatusCode();
        
        var data = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<GetCepResponse>(data)!;

        if (json.Erro)
            return null;

        return new GetCepModel()
        {
            Cep = json.Cep,
            Logradouro = json.Logradouro,
            Complemento = json.Complemento,
            Bairro = json.Bairro,
            Cidade = json.Localidade,
            Uf = json.Uf
        };
    }
}
