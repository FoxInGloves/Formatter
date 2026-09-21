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
        document = DocX.Load($@"{directory}\DocumentTemplates\1.docx");
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

        document.ReplaceText("{{ДатаПИполн}}", NoticeDate, false, RegexOptions.None, formating);
        document.ReplaceText("{{ДатаПИ}}", NoticeDate.Remove(5, 5), false, RegexOptions.None, formating);

        document.ReplaceText("{{Номер договора}}", ContractNumber, false, RegexOptions.None, formating);
        document.ReplaceText("{{ДатаОт}}", ContractDate, false, RegexOptions.None, formating);
        var contractDay = ContractDate.Substring(0, 2);
        var contractMonth = ContractDate.Substring(3, 2);
        var contractYear = ContractDate.Substring(8, 2);
        document.ReplaceText("{{День договора}}", contractDay, false, RegexOptions.None, formating);
        document.ReplaceText("{{Месяц договора}}", contractMonth, false, RegexOptions.None, formating);
        document.ReplaceText("{{Год договора}}", contractYear, false, RegexOptions.None, formating);

        document.ReplaceText("{{Название детали}}", DetailName, false, RegexOptions.None, formating);
        document.ReplaceText("{{Количество}}", DetailCount.ToString, false, RegexOptions.None, formating);
        document.ReplaceText("{{Заводской номер}}", SerialNumber, false, RegexOptions.None, formating);
        document.ReplaceText("{{Инвентарный номер}}", InventoryNumber, false, RegexOptions.None, formating);
        
        document.ReplaceText("{{ДатаПСИполн}}", PSIDate, false, RegexOptions.None, formating);
        document.ReplaceText("{{ДатаПСИ}}", PSIDate.Remove(5, 5), false, RegexOptions.None, formating);
        document.ReplaceText("{{НомерПСИ}}", PSINumber, false, RegexOptions.None, formating);
        
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


    /*public void ReplaceTextInWord(string filePath, string targetText, string replacementText)
    {
        var appSettings = App.Services.GetRequiredService<AppSettings>();

        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
        {
            XWPFDocument doc = new XWPFDocument(file);

            // Обходим все параграфы в документе
            foreach (var paragraph in doc.Paragraphs)
            {
                ReplaceInParagraph(paragraph, targetText, replacementText);
            }

            // Также можно пройти по таблицам, если текст находится в них
            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var paragraph in cell.Paragraphs)
                        {
                            ReplaceInParagraph(paragraph, targetText, replacementText);
                        }
                    }
                }
            }

            // Сохраняем изменения в тот же файл или новый
            using (FileStream outStream = new FileStream($"{appSettings.PathForSaveDocument}/{FileName}.doc", FileMode.Create, FileAccess.Write))
            {
                doc.Write(outStream);
            }
        }
    }

    private void ReplaceInParagraph(XWPFParagraph paragraph, string targetText, string replacementText)
    {
        // Проверяем, содержит ли параграф искомый текст
        if (paragraph.Text.Contains(targetText))
        {
            foreach (var run in paragraph.Runs)
            {
                string text = run.ToString();
                if (text.Contains(targetText))
                {
                    run.SetText(text.Replace(targetText, replacementText), 0);
                }
            }
        }
    }*/
}