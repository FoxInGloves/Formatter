using Formatter.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Formatter.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPageViewModel ViewModel { get; }
    
    public SettingsPage()
    {
        InitializeComponent();
        
        ViewModel = App.Services.GetRequiredService<SettingsPageViewModel>();
    }
}