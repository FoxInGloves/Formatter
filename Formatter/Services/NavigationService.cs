using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace Formatter.Services;

public class NavigationService : INavigationService
{
    private Frame? _frame;

    public bool CanGoBack => _frame?.CanGoBack ?? false;

    public void Initialize(Frame shellFrame)
    {
        _frame = shellFrame;
    }

    public bool Navigate(Type pageType, object? parameter = null)
    {
        if (_frame == null) return false;
        
        if (_frame.CurrentSourcePageType == pageType && parameter == null) return false;

        return _frame.Navigate(pageType, parameter);
    }

    public bool GoBack()
    {
        if (CanGoBack)
        {
            _frame?.GoBack();
            return true;
        }
        return false;
    }
}