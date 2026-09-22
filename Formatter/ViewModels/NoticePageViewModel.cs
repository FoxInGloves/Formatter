using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Formatter.Models;
using Microsoft.Extensions.DependencyInjection;
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
    private string? _detailName;

    private DocX document;

    private readonly Dictionary<string, string> _detailNameInventoryNumber = new()
    {
        { "Деталь 1", "111" }, { "Деталь 2", "222" }, { "Деталь 3", "333" }, { "Деталь 4", "444" }
    };

    public NoticePageViewModel()
    {
        var directory = AppDomain.CurrentDomain.BaseDirectory;
        document = DocX.Load($@"{directory}\DocumentTemplates\Notice.docx");
    }

    [ObservableProperty] public partial string? FileName { get; set; }

    [ObservableProperty] public partial string? NoticeDate { get; set; }

    [ObservableProperty] public partial string? ContractNumber { get; set; }


    [ObservableProperty] public partial string? ContractDate { get; set; }

    public string? DetailName
    {
        get => _detailName;
        set
        {
            if (_detailName == value) return;

            _detailName = value;
            OnPropertyChanged();

            if (_detailNameInventoryNumber.TryGetValue(value, out var value1))
                InventoryNumber = value1;
        }
    }

    [ObservableProperty] public partial int DetailCount { get; set; }

    [ObservableProperty] public partial string? SerialNumber { get; set; }

    [ObservableProperty] public partial string? InventoryNumber { get; set; }
    
    [ObservableProperty] public partial string PSIDate { get; set; }
    
    [ObservableProperty] public partial string PSINumber { get; set; }

    public string[] DetailNamesArray { get; private set; } = ["Деталь 1", "Деталь 2", "Деталь 3", "Деталь 4"];

    [RelayCommand]
    private void Confirm()
    {
        var formating = new Formatting();
        formating.Highlight = Highlight.red;

        document.ReplaceText("{{ДатаПИполн}}", NoticeDate);
        document.ReplaceText("{{ДатаПИ}}", NoticeDate.Remove(5, 5));

        document.ReplaceText("{{Номер договора}}", ContractNumber);
        document.ReplaceText("{{ДатаОт}}", ContractDate);
        var contractDay = ContractDate.Substring(0, 2);
        var contractMonth = ContractDate.Substring(3, 2);
        var contractYear = ContractDate.Substring(8, 2);
        document.ReplaceText("{{День договора}}", contractDay);
        document.ReplaceText("{{Месяц договора}}", contractMonth);
        document.ReplaceText("{{Год договора}}", contractYear);

        document.ReplaceText("{{Название детали}}", DetailName);
        document.ReplaceText("{{Количество}}", DetailCount.ToString);
        document.ReplaceText("{{Заводской номер}}", SerialNumber);
        document.ReplaceText("{{Инвентарный номер}}", InventoryNumber);
        
        document.ReplaceText("{{ДатаПСИполн}}", PSIDate);
        document.ReplaceText("{{ДатаПСИ}}", PSIDate.Remove(5, 5));
        document.ReplaceText("{{НомерПСИ}}", PSINumber);
        
        if (string.IsNullOrEmpty(FileName))
        {
            FileName = "Результат";
        }

        if (string.IsNullOrEmpty(FileName))
        {
            FileName = "Результат";
        }

        var appSettings = App.Services.GetRequiredService<AppSettings>();

        document.SaveAs($"{appSettings.PathForSaveDocument}/{FileName}.docx");
    }
}