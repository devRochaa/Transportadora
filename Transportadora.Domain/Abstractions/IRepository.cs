using System.Linq.Expressions;

namespace Transportadora.Domain.Abstractions;

public interface IRepository<TEntity>
    where TEntity : class
{

    // retorno de entidades completas
    Task<TEntity?> FirstOrDefaultAsync(
    Expression<Func<TEntity, bool>> where,
    bool tracking = true,
    CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TEntity>> ToListAsync(
        Expression<Func<TEntity, bool>>? where = null,
        bool tracking = false,
        CancellationToken cancellationToken = default);

    // retorno de projections (dtos)
    Task<TProjection?> FirstOrDefaultAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TProjection>> ToListAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> selector,
        Expression<Func<TEntity, bool>>? where = null,
        bool tracking = false,
        CancellationToken cancellationToken = default);

    //outros
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default);

    // alteração
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entity);
    void Remove(TEntity entity);

}
