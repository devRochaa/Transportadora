using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Transportadora.Domain.Abstractions;
using Transportadora.Infrasctructure.Context;

namespace Transportadora.Infrasctructure.Repositories;

public class EfRepository<TEntity>(AppDbContext context) : IRepository<TEntity>
        where TEntity : class, IEntity
{
    protected readonly AppDbContext _dbContext = context;

    // --- ENTIDADE COMPLETA ---
    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> where,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {

        var query = _dbContext
            .Set<TEntity>()
            .Where(where);

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? where = null, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        if (where is not null)
            query = query.Where(where);

        return await query.ToListAsync(cancellationToken);
    }

    // --- PROJECTION ---
    public Task<TProjection?> FirstOrDefaultAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .Where(where)
            .Select(selector);

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TProjection>> ToListAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>>? where = null,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext
            .Set<TEntity>()
            .AsQueryable();

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        if (where is not null)
        {
            query = query.Where(where);
        }

        return await query.Select(selector).ToListAsync(cancellationToken);
    }
    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where,
                               CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .AnyAsync(where, cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return _dbContext
             .Set<TEntity>()
             .AddRangeAsync(entities, cancellationToken);
    }
    public void Update(TEntity entity) =>
        _dbContext.Set<TEntity>().Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) =>
        _dbContext.Set<TEntity>().UpdateRange(entities);

    public void Remove(TEntity entity) =>
        _dbContext.Set<TEntity>().Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) =>
        _dbContext.Set<TEntity>().RemoveRange(entities);


}

