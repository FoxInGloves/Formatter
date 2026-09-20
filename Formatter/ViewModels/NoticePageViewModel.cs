using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinRT.Interop;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Formatter.ViewModels;

/// <summary>
/// Sample ViewModel using CommunityToolkit.Mvvm partial property syntax.
/// Uses <see cref="ObservableProperty"/> for change notification and
/// <see cref="RelayCommand"/> for command binding.
/// </summary>
public partial class NoticePageViewModel : ObservableObject
{
    private DocX document;
    
    public NoticePageViewModel()
    {
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        document = DocX.Load($@"{desktop}\Test\1.docx");
    }
    /*[ObservableProperty] public partial string Greeting { get; set; } = "Hello, WinUI!";

    [ObservableProperty] public partial int Counter { get; set; }

    [RelayCommand]
    private void Increment()
    {
        Counter++;
    }

    [RelayCommand]
    private void Decrement()
    {
        Counter--;
    }*/
    
    [ObservableProperty] public partial string? FileName { get; set; }
    
    [ObservableProperty] public partial string? NoticeDate { get; set; }
    
    [ObservableProperty] public partial string? ContractNumber { get; set; }
    
    
    [ObservableProperty] public partial string? ContractDate { get; set; }
    
    [ObservableProperty] public partial string? DetailName { get; set; }
    
    [ObservableProperty] public partial int DetailCount { get; set; }
    
    [ObservableProperty] public partial string? SerialNumber { get; set; }
    
    [ObservableProperty] public partial string PSIDate { get; set; }
    
    [ObservableProperty] public partial string? InventoryNumber { get; set; }

    public string[] DetailNamesArray { get; private set; } = ["Деталь 1", "Деталь 2", "Деталь 3", "Деталь 4"];

    [RelayCommand]
    private void Confirm()
    {
        var formating = new Formatting();
        formating.Highlight = Highlight.red;
        
        document.ReplaceText("{{Номер договора}}", ContractNumber, false, RegexOptions.None, formating);
        document.ReplaceText("{{ДатаОт}}", ContractDate, false, RegexOptions.None, formating);
        document.ReplaceText("{{Название детали}}", DetailName, false, RegexOptions.None, formating);
        document.ReplaceText("{{Заводской номер}}", SerialNumber, false, RegexOptions.None, formating);
        document.ReplaceText("{{Инвентарный номер}}", InventoryNumber, false, RegexOptions.None, formating);
        document.ReplaceText("{{ДатаПСИполн}}", PSIDate, false, RegexOptions.None, formating);
        
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        if (string.IsNullOrEmpty(FileName))
        {
            FileName = "Результат";
        }
        document.SaveAs($@"{desktop}\Test\{FileName}.docx");
    }
}