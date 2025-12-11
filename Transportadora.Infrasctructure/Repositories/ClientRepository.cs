using Transportadora.Domain.Entities;
using Transportadora.Infrasctructure.Context;

namespace Transportadora.Infrasctructure.Repositories;

public class ClientRepository(AppDbContext context) : EfRepository<PersonEntity>(context)
{
}
