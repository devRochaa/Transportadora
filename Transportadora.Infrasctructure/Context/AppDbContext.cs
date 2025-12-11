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

    private void BeforeSave(){}
}

