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

    [ObservableProperty] private NavigationItem _selectedMenuItem;

    [ObservableProperty] private bool _isSettingsSelected;

    public ObservableCollection<NavigationItem> NavigationItems { get; }


    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        NavigationItems = new ObservableCollection<NavigationItem>
        {
            new() { Title = "Извещения", IconGlyph = "\uE6C2", TargetPageType = typeof(NoticePage) }
        };

        _selectedMenuItem = NavigationItems[0];
    }
    
    public void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            SelectedMenuItem = null;
            _navigationService.Navigate(typeof(SettingsPage));
            return;
        }

        if (args.InvokedItemContainer is NavigationViewItem invokedItem && 
            invokedItem.Tag is Type targetPageType)
        {
            SelectedMenuItem = NavigationItems.FirstOrDefault(m => m.TargetPageType == targetPageType);
            
            _navigationService.Navigate(targetPageType);
        }
    }
}