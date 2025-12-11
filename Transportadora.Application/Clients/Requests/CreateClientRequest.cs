using FluentValidation;
using System.Text.Json.Serialization;

namespace Transportadora.Application.Clients.Requests;

public sealed record CreateClientRequest(
     string Fullname,
     string NationalDocument,
     string Phone,
     DateTime BirthDate
);
