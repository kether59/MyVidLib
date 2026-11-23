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

        // Act
        var result = services.AddInfrastructure();

        // Assert
        Assert.Same(services, result);
    }
}
