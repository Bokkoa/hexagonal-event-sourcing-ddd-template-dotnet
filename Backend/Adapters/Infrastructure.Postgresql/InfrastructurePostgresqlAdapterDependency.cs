using Application.Abstractions.Ports.Repositories;
using Infrastructure.Common.DataAccess;
using Infrastructure.Common.Repositories;
using Infrastructure.Postgresql.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Postgresql;
public static class InfrastructurePostgresqlAdapterDependency
{
    public static IServiceCollection InjectPostgresqlDependency(this IServiceCollection services, IConfiguration configuration)
    {

        Action<DbContextOptionsBuilder> configureDbContext;

        configureDbContext = o => o.UseLazyLoadingProxies()
                                    .UseNpgsql(configuration.GetConnectionString("Postgresql"));

        services.AddDbContext<DatabaseContext>(configureDbContext);

        var dataContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        dataContext.Database.EnsureCreated();

        services.AddScoped<ITodoRepository, TodoRepository>();

        services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

        return services;

    }
}
