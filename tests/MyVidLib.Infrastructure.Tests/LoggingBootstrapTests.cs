using MyVidLib.Infrastructure.Logging;
using Serilog;
using Serilog.Events;

namespace MyVidLib.Infrastructure.Tests;

public class LoggingBootstrapTests
{
    [Fact]
    public void CreateDefaultConfiguration_ReturnsNonNullConfiguration()
    {
        // Act
        var config = LoggingBootstrap.CreateDefaultConfiguration();

        // Assert
        Assert.NotNull(config);
    }

    [Fact]
    public void CreateDefaultConfiguration_EnablesDebugLevel()
    {
        // Act
        using var logger = LoggingBootstrap.CreateDefaultConfiguration().CreateLogger();

        // Assert
        Assert.True(logger.IsEnabled(Serilog.Events.LogEventLevel.Debug));
    }

    [Fact]
    public void CreateDefaultConfiguration_InformationLevelIsEnabled()
    {
        // Act
        using var logger = LoggingBootstrap.CreateDefaultConfiguration().CreateLogger();

        // Assert
        Assert.True(logger.IsEnabled(Serilog.Events.LogEventLevel.Information));
    }

    [Fact]
    public void CreateDefaultConfiguration_WritesInformationWithoutThrowing()
    {
        // Act / Assert
        using var logger = LoggingBootstrap.CreateDefaultConfiguration().CreateLogger();
        var exception = Record.Exception(() => logger.Information("Test information message"));

        Assert.Null(exception);
    }

    [Fact]
    public void CreateDefaultConfiguration_WritesDebugWithoutThrowing()
    {
        // Act / Assert
        using var logger = LoggingBootstrap.CreateDefaultConfiguration().CreateLogger();
        var exception = Record.Exception(() => logger.Debug("Test debug message"));

        Assert.Null(exception);
    }
}