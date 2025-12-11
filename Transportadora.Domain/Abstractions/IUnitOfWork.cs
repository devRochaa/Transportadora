namespace Transportadora.Domain.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{

    Task BeginTransactionAsync(CancellationToken cancellation = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellation = default);

    Task RollbackAsync(CancellationToken cancellation = default);
}
