using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace MyVidLib.Wpf.ViewModels;

/// <summary>
/// Minimal main ViewModel for M0. Verifies MVVM, DI and logging.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly ILogger<MainViewModel> _logger;

    [ObservableProperty]
    private string _statusMessage = "MyVidLib is ready (M0 – technical initialization).";

    [ObservableProperty]
    private string _versionInfo = "M0 – Technical Initialization";

    public MainViewModel(ILogger<MainViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("MainViewModel created.");
    }

    [RelayCommand]
    private void RefreshStatus()
    {
        StatusMessage = $"Status refreshed at {DateTime.Now:HH:mm:ss}.";
        _logger.LogInformation("Status refreshed by user.");
    }
}
