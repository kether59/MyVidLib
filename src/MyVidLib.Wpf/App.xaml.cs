using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyVidLib.Infrastructure.DependencyInjection;
using MyVidLib.Infrastructure.Logging;
using MyVidLib.Wpf.ViewModels;
using Serilog;

namespace MyVidLib.Wpf;

/// <summary>
/// Composition root for the MyVidLib WPF application (M0).
/// Configures Host, DI, Serilog and unhandled exception handling.
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

        try
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var logDirectory = Path.Combine(appData, "MyVidLib", "Logs");
            Directory.CreateDirectory(logDirectory);

            // Single source of truth for base configuration (level, enrich, console sink).
            // File sink added here (Wpf composition root only).
            Log.Logger = LoggingBootstrap.CreateDefaultConfiguration()
                .WriteTo.File(
                    path: Path.Combine(logDirectory, "myvidlib-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            _host = Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructure();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();

            await _host.StartAsync().ConfigureAwait(true);

            var logger = _host.Services.GetRequiredService<ILogger<App>>();
            logger.LogInformation("MyVidLib starting (M0 – technical initialization).");

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application failed to start.");
            MessageBox.Show(
                "Unable to start MyVidLib. See logs for details.",
                "MyVidLib – Startup Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        try
        {
            if (_host is not null)
            {
                var logger = _host.Services.GetService<ILogger<App>>();
                logger?.LogInformation("MyVidLib shutting down.");

                try
                {
                    await _host.StopAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    // Never swallow silently: log and continue to dispose/flush.
                    Log.Error(ex, "Host StopAsync failed during shutdown.");
                }

                _host.Dispose();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while shutting down the host.");
        }
        finally
        {
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Unhandled dispatcher exception.");
        e.Handled = true;
        MessageBox.Show(
            "An unexpected error occurred. The application will continue if possible.",
            "MyVidLib – Error",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        try
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log.Fatal(ex, "Unhandled AppDomain exception. IsTerminating={IsTerminating}", e.IsTerminating);
            }
            else
            {
                Log.Fatal("Unhandled AppDomain exception (non-Exception). IsTerminating={IsTerminating}", e.IsTerminating);
            }
        }
        finally
        {
            // Flush immediately: process may terminate right after this event.
            try { Log.CloseAndFlush(); } catch { /* best effort on fatal path */ }
        }
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Unobserved task exception.");
        e.SetObserved();
    }
}
