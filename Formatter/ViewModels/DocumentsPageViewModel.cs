using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Formatter.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Formatter.ViewModels;

/// <summary>
/// Sample ViewModel using CommunityToolkit.Mvvm partial property syntax.
/// Uses <see cref="ObservableProperty"/> for change notification and
/// <see cref="RelayCommand"/> for command binding.
/// </summary>
public partial class DocumentsPageViewModel : ObservableObject
{
    private DocX? _document;
    
    [GeneratedRegex("{{[^}]+}}")]
    private static partial Regex MyRegex();

    public DocumentsPageViewModel()
    {
        Log.Information("NoticePageViewModel created");
    }

    public string? PathToFile
    {
        get;

        set
        {
            Log.Information("PathToFile set to {value}", value);
            if (value == null)
            {
                IsFileSelected = false;
                return;
            }

            if (field == value) return;

            field = value;
            OnPropertyChanged();

            IsFileSelected = true;

            _document = DocX.Load(field);

            var fullText = _document.Text;
            var regex = MyRegex();

            var matches = regex.Matches(fullText);
            var uniqueMatches = matches
                .Select(m => m.Value)
                .Distinct()
                .ToArray();

            Log.Information("Unique matches found: {count}", uniqueMatches.Length);

            foreach (var match in uniqueMatches)
            {
                var fieldName = match.Substring(2, match.Length - 4);

                Fields.Add(new FieldModel
                {
                    FullName = match,
                    FieldName = fieldName
                });
            }
        }
    }

    [ObservableProperty] public partial string ErrorText { get; private set; } = "";
    
    [ObservableProperty]
    public partial bool IsFileSelected { get; private set; }
    
    [ObservableProperty] public partial string? FileName { get; set; }

    public ObservableCollection<FieldModel> Fields { get; set; } = [];

    [RelayCommand]
    private async Task OpenFileDialog()
    {
        var filePicker = new FileOpenPicker();
        filePicker.FileTypeFilter.Add(".docx");
        
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);  

        var file = await filePicker.PickSingleFileAsync();
        PathToFile = file?.Path;
    }
    
    [RelayCommand]
    private void Confirm()
    {
        if (_document is null)
        {
            ErrorText = "Не удалось загрузить документ";
            Log.Error("Document is null");
            return;
        }

        if (!string.IsNullOrEmpty(ErrorText))
        {
            ErrorText = string.Empty;
        }

        var formating = new Formatting
        {
            Highlight = Highlight.red
        };

        foreach (var field in Fields)
        {
            _document.ReplaceText(field.FullName, field.Value,  false, RegexOptions.None,  formating);
        }
        
        if (string.IsNullOrEmpty(FileName))
        {
            FileName = "Результат";
        }

        if (string.IsNullOrEmpty(FileName))
        {
            FileName = "Результат";
        }

        var appSettings = App.Services.GetRequiredService<AppSettings>();

        _document.SaveAs($"{appSettings.PathForSaveDocument}/{FileName}.docx");
    }
}