using System.Text.Json;
using System.Text.Json.Nodes;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Formatter.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Formatter.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty] private string? _folderPath;
    
    private AppSettings _appSettings;

    public SettingsPageViewModel(AppSettings appSettings)
    {
        _appSettings = appSettings;
        _folderPath = appSettings.PathForSaveDocument;
    }
    
    [RelayCommand]
    private async Task OpenFolderDialog()
    {
        var folderPicker = new FolderPicker();
        folderPicker.FileTypeFilter.Add("*");
        
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);  

        var folder = await folderPicker.PickSingleFolderAsync();
        FolderPath = folder.Path;
    }

    [RelayCommand]
    private void SaveSettings()
    {
        var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //var directoryToFile = currentDirectory.Replace("ViewModels", "");
        var filePath = $"{currentDirectory}/AppSettings.json";
       
        _appSettings.PathForSaveDocument = FolderPath;
        
        var jsonString = JsonSerializer.Serialize(_appSettings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, jsonString);
    }
}