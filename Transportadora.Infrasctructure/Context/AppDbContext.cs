using Microsoft.EntityFrameworkCore;

namespace Transportadora.Infrasctructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(modelBuilder: builder);

        //isso faz o EF procurar automaticamente todas as classes que implementam IEntityTypeConfiguration<T>
        //para fazer os mapeamentos separados das entitys
        builder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }

    public override int SaveChanges()
    {
        BeforeSave();
        return base.SaveChanges();
    }


    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        BeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        BeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        BeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public void BeforeSave()
    {
        EntityState[] entityStates = [
            EntityState.Added,
            EntityState.Modified,
            EntityState.Deleted
        ];
        var dateTrackedEntities = ChangeTracker
            .Entries()
            .Where(e => entityStates.Contains(e.State));

        var now = DateTimeOffset.Now;
        foreach (var entry in dateTrackedEntities)
        {
            var entity = entry.Entity;

            switch (entry.State)
            {
                case EntityState.Deleted:
                    if (entity is ISoftDelete softDelete)
                    {
                        softDelete.IsDeleted = true;

                        if (softDelete is IDateTracked dateTrackingDeleted)
                            dateTrackingDeleted.LastUpdatedAt = now;

                        entry.State = EntityState.Modified;
                    }
                    break;

                case EntityState.Modified:
                    if (entity is IDateTracked dateTrackingModified)
                        dateTrackingModified.LastUpdatedAt = now;
                    break;

                case EntityState.Added:
                    if (entity is IDateTracked dateTrackingAdded)
                    {
                        dateTrackingAdded.CreatedAt = now;
                        dateTrackingAdded.LastUpdatedAt = now;
                    }
                    break;
            }

        }
    }

