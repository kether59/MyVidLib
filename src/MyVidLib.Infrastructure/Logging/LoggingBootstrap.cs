using Serilog;
using Serilog.Events;

namespace MyVidLib.Infrastructure.Logging;

/// <summary>
/// Minimal Serilog bootstrap helpers used by the composition root.
/// Full configuration (paths, retention) is applied from the WPF host in M0.
/// </summary>
public static class LoggingBootstrap
{
    public static LoggerConfiguration CreateDefaultConfiguration()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
    }
}
