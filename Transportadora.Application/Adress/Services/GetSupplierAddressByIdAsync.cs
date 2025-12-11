using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Entities;

namespace Transportadora.Application.Adress.Services;

public sealed class GetSupplierAddressByIdAsync(IRepository<AddressEntity> repository, IUnitOfWork unitOfWork) : ServiceBase<AddressEntity>(repository, unitOfWork)
{
    public async Task<AddressEntity?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await repository.FirstOrDefaultAsync(e => e.SupplierId == id,
                                                             tracking: false,
                                                             cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Supplier address not found.");

        return supplier;
    }
}
