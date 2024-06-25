using Application.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;

namespace Persistence;

//This class is used to register services from infrastructure layer

public static class PersistenceExtensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDBContext>(options =>
        {
            //We don't register the DB connection here, we set the database connection in the OnConfiguring() method inside the app db context instead
            //var connectionString = configuration.GetConnectionString("DBConnection");
            //options.UseSqlServer(connectionString);
        });

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }
}
