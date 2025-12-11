namespace Transportadora.Application.Clients.Requests;

public sealed class UpdateClientRequest
{
    public required string Fullname { get; set; }
    public required string NationalDocument { get; set; }
    public required string Phone { get; set; }
    public required DateTime BirthDate { get; set; }
}
