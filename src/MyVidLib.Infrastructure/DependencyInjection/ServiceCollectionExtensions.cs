using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyVidLib.Infrastructure.Persistence;

namespace MyVidLib.Infrastructure.DependencyInjection;

/// <summary>
/// Extension methods to register Infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MyVidLibDbContext>(options =>
        {
            options.UseSqlite(connectionString);
            options.EnableSensitiveDataLogging(false);
        });

        return services;
    }
}
