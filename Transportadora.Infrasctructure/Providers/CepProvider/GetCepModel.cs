namespace Transportadora.Infrasctructure.Providers.CepProvider;

public sealed class GetCepModel
{
    public required string Cep { get; set; }
    public required string Logradouro { get; set; }
    public required string Complemento { get; set; }
    public required string Bairro { get; set; }
    public required string Cidade { get; set; }
    public required string Uf { get; set; }
}
