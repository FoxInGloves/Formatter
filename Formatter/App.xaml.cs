using System.Text.Json;
using Formatter.Models;
using Formatter.Services;
using Formatter.ViewModels;
using Formatter.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formatter;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// The main application window. Use <c>App.Window</c> from any class that needs
    /// the window reference (for dialogs, pickers, interop, etc.).
    /// </summary>
    public static Window Window { get; private set; } = null!;

    /// <summary>
    /// The UI thread dispatcher. Use <c>App.DispatcherQueue</c> to marshal calls
    /// to the UI thread. Fully qualified to avoid CS0104 ambiguity with
    /// <see cref="Windows.System.DispatcherQueue"/>.
    /// </summary>
    public static Microsoft.UI.Dispatching.DispatcherQueue DispatcherQueue { get; private set; } = null!;

    /// <summary>
    /// The native window handle (HWND). Use for file pickers,
    /// <c>DataTransferManager</c>, and any WinRT interop that requires
    /// <c>InitializeWithWindow</c>.
    /// </summary>
    public static nint WindowHandle =>
        WinRT.Interop.WindowNative.GetWindowHandle(Window);
    
    public static IServiceProvider Services { get; private set; }

    /// <summary>
    /// Initializes the singleton application object.
    /// </summary>
    public App()
    {
        InitializeComponent();

        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddTransient<MainWindowViewModel>();
        serviceCollection.AddTransient<NoticePageViewModel>();
        serviceCollection.AddTransient<SettingsPageViewModel>();
        
        serviceCollection.AddSingleton<INavigationService, NavigationService>();

        var directory = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = $"{directory}AppSettings.json";

        var readJson = File.ReadAllText(filePath);
        var appSettings = JsonSerializer.Deserialize<AppSettings>(readJson);
        serviceCollection.AddSingleton<AppSettings>(appSettings);

        Services = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Services.GetRequiredService<MainWindowViewModel>();
        Window = new MainWindow();
        DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        Window.Activate();
    }
}