using System.Windows;
using MyVidLib.Wpf.ViewModels;

namespace MyVidLib.Wpf;

/// <summary>
/// Main window code-behind. DataContext is injected via DI.
/// No business logic lives here.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
