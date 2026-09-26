using CommunityToolkit.Mvvm.ComponentModel;

namespace Formatter.Models;

public partial class FieldModel : ObservableObject
{
    public string FullName { get; set; } = "";
    
    public string FieldName { get; set; } = "";
    public FieldType Type { get; set; }

    [ObservableProperty] public partial string? Value { get; set; }
    
    public bool BoolValue { get; set; }
}