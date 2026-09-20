using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Formatter.Services;
using Formatter.Views;

namespace Formatter.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    
    public void NavigateToTag(string? pageTag)
    {
        var pageType = pageTag switch
        {
            "NoticedPage" => typeof(NoticePage),
            _ => typeof(NoticePage)
        };

        _navigationService.Navigate(pageType);
    }
}