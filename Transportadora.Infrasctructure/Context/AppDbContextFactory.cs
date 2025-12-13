using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Transportadora.Infrasctructure.Context;
internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            connectionString: "Server=(localdb)\\mssqllocaldb;Database=transportadora;Trusted_Connection=True;TrustServerCertificate=True"
            //    sqlServerOption.MigrationsAssembly(typeof(DependencyInjection).Namespace);
            //}
            );

        return new AppDbContext(optionsBuilder.Options);
    }
}
