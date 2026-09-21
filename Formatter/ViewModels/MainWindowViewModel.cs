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
            new() { Title = "Извещения", IconGlyph = "\uE80F", TargetPageType = typeof(NoticePage) }
        };

        _selectedMenuItem = NavigationItems[0];
    }
    
    public void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        // 1. Проверяем, кликнули ли на настройки
        if (args.IsSettingsInvoked)
        {
            SelectedMenuItem = null;
            _navigationService.Navigate(typeof(SettingsPage));
            return;
        }

        // 2. Если это обычный пункт меню
        if (args.InvokedItemContainer is NavigationViewItem invokedItem && 
            invokedItem.Tag is Type targetPageType)
        {
            // Синхронизируем подсветку выбранного пункта во ViewModel
            SelectedMenuItem = NavigationItems.FirstOrDefault(m => m.TargetPageType == targetPageType);
            
            // Выполняем навигацию
            _navigationService.Navigate(targetPageType);
        }
    }
}