using Transportadora.Application.Clients.Requests;
using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Entities;

namespace Transportadora.Application.Clients.Services;

public sealed class UpdateClientService(IRepository<PersonEntity> repository, IUnitOfWork unitOfWork) : ServiceBase<PersonEntity>(repository, unitOfWork)
{
    public async Task ExecuteAsync(Guid clientId, UpdateClientRequest req, CancellationToken cancellationToken)
    {
        PersonEntity client = await repository
            .FirstOrDefaultAsync(c => c.Id == clientId, true, cancellationToken)
            ?? throw new InvalidOperationException($"Client with ID {clientId} not found.");

        client.Fullname = req.Fullname;
        client.NationalDocument = req.NationalDocument;
        client.Phone = req.Phone;
        client.BirthDate = DateOnly.FromDateTime(req.BirthDate);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        repository.Update(client);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
