using Transportadora.Application.Clients.Models;
using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Entities;

namespace Transportadora.Application.Clients.Services;

public class GetClientService(IRepository<PersonEntity> repository, IUnitOfWork unitOfWork) : ServiceBase<PersonEntity>(repository, unitOfWork)
{
    public async Task<List<GetClientModel>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var clients = await repository.ToListAsync(tracking: false, cancellationToken: cancellationToken);

        return clients
            .Select(e => new GetClientModel {
                Id = e.Id,
                Fullname = e.Fullname,
                NationalDocument = e.NationalDocument,
                Phone = e.Phone,
                BirthDate = e.BirthDate
            })
            .ToList();
    }
}
