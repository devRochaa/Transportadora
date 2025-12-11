using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Entities;

namespace Transportadora.Application.Supplier.Services;

public sealed class GetSuppliersService(IRepository<SupplierEntity> _repository, IUnitOfWork _unitOfWork) : ServiceBase<SupplierEntity>(_repository, _unitOfWork)
{
    public async Task<IEnumerable<SupplierEntity>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var suppliers = await _repository.ToListAsync(tracking: false, cancellationToken: cancellationToken);
        return suppliers;
    }
}
