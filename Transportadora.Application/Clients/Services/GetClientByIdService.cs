using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Entities;

namespace Transportadora.Application.Clients.Services;

public sealed class GetClientByIdService(IRepository<PersonEntity> repository, IUnitOfWork unitOfWork) : ServiceBase<PersonEntity>(repository, unitOfWork)
{
    public async Task<PersonEntity?> ExecuteAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var client = await repository.FirstOrDefaultAsync(c => c.Id == clientId, false, cancellationToken);
        return client;
    }
}
