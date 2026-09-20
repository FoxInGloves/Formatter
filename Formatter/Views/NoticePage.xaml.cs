using Formatter.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formatter.Views;

/// <summary>
/// The main content page displayed inside the application window.
/// </summary>
public sealed partial class NoticePage : Page
{
    public NoticePageViewModel ViewModel { get; }

    public NoticePage()
    {
        InitializeComponent();
        
        ViewModel = App.Services.GetRequiredService<NoticePageViewModel>();
    }
}