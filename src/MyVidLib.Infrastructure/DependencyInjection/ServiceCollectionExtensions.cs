using Microsoft.Extensions.DependencyInjection;

namespace MyVidLib.Infrastructure.DependencyInjection;

/// <summary>
/// Extension methods to register Infrastructure services.
/// M0 only provides the composition entry point; concrete services will be added in later milestones.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // M0: no concrete infrastructure services yet.
        // Future: EF Core, file system, FFprobe, providers, etc.
        return services;
    }
}
