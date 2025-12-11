using Microsoft.EntityFrameworkCore.Storage;
using Transportadora.Domain.Abstractions;
using Transportadora.Infrasctructure.Context;

namespace Transportadora.Infrasctructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    private int _transactionDepth;

    public async Task BeginTransactionAsync(CancellationToken cancellation = default)
    {
        if (_transactionDepth == 0)
        {
            _currentTransaction = await context.Database.BeginTransactionAsync(cancellation);
        }
        _transactionDepth++;
    }

    public async Task CommitAsync(CancellationToken cancellation = default)
    {
        //tenta salvar as mudanças antes de commitar a transação
        await context.SaveChangesAsync(cancellation);

        if (_transactionDepth <= 0 || _currentTransaction == null)
            return;

        _transactionDepth--;

        if (_transactionDepth == 0)
        {
            await _currentTransaction.CommitAsync(cancellation);
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellation = default)
    {
        if (_transactionDepth <= 0 || _currentTransaction == null)
            return;

        _transactionDepth = 0;

        await _currentTransaction.RollbackAsync(cancellation);
        await DisposeTransactionAsync();

        //ChangeTracker rastreia todas as entidades carregadas no contexto (AppDbContext)
        //.Clear() limpa completamente o rastreamento de todas as entidades no contexto.
        context.ChangeTracker.Clear();
    }

    public async ValueTask DisposeTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public ValueTask DisposeAsync() => context.DisposeAsync();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}
