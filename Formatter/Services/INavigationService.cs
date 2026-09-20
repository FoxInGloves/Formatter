using Microsoft.UI.Xaml.Controls;

namespace Formatter.Services;

public interface INavigationService
{
    void Initialize(Frame shellFrame);
    bool Navigate(Type pageType, object? parameter = null);
    bool GoBack();
    bool CanGoBack { get; }
}