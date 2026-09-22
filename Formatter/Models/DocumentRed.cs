using CommunityToolkit.Mvvm.ComponentModel;

namespace Formatter.Models;

public partial class DocumentRed : ObservableObject
{
    public string FieldName { get; set; }
    
    [ObservableProperty] public partial string NewText { get; set; }
}