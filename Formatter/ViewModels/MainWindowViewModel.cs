using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Formatter.Models;
using Formatter.Services;
using Formatter.Views;
using Microsoft.UI.Xaml.Controls;

namespace Formatter.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty] public partial NavigationItem? SelectedMenuItem { get; set; }

    public ObservableCollection<NavigationItem> NavigationItems { get; }


    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        NavigationItems =
        [
            new NavigationItem { Title = "Документы", TargetPageType = typeof(DocumentsPage) }
        ];

        SelectedMenuItem = NavigationItems[0];
    }
    
    public void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            SelectedMenuItem = null;
            _navigationService.Navigate(typeof(SettingsPage));
            return;
        }

        if (args.InvokedItemContainer is NavigationViewItem { Tag: Type targetPageType })
        {
            SelectedMenuItem = NavigationItems.FirstOrDefault(m => m.TargetPageType == targetPageType);
            
            _navigationService.Navigate(targetPageType);
        }
    }
}