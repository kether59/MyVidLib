using Microsoft.Extensions.DependencyInjection;
using MyVidLib.Infrastructure.DependencyInjection;

namespace MyVidLib.Infrastructure.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddInfrastructure_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var connectionString = "Data Source=:memory:";

        // Act
        var result = services.AddInfrastructure(connectionString);

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddInfrastructure_DoesNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();
        var connectionString = "Data Source=:memory:";

        // Act / Assert
        var exception = Record.Exception(() => services.AddInfrastructure(connectionString));

        Assert.Null(exception);
    }

    [Fact]
    public void AddInfrastructure_CanBuildServiceProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        var connectionString = "Data Source=:memory:";

        // Act
        services.AddInfrastructure(connectionString);
        using var provider = services.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        // Assert
        Assert.NotNull(provider);
    }
}