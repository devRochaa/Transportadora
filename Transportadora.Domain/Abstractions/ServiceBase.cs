namespace Transportadora.Domain.Abstractions;

public abstract class ServiceBase<TEntity>
    where TEntity : class, IEntity
{
    protected readonly IRepository<TEntity> repository;
    protected readonly IUnitOfWork unitOfWork;

    protected ServiceBase(IRepository<TEntity> _repository, IUnitOfWork _unitOfWork)
    {
        repository = _repository;
        unitOfWork = _unitOfWork;
    }
}
