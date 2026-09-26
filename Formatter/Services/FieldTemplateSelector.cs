using Formatter.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Formatter.Services;

public class FieldTemplateSelector : DataTemplateSelector
{
    public DataTemplate? TextTemplate { get; set; }
    public DataTemplate? NumberTemplate { get; set; }
    
    public DataTemplate? DateTemplate { get; set; }
    public DataTemplate? BooleanTemplate { get; set; }

    protected override DataTemplate? SelectTemplateCore(
        object item)
    {
        if (item is not FieldModel field)
            return null;

        return field.Type switch
        {
            FieldType.Text => TextTemplate,
            FieldType.Number => NumberTemplate,
            FieldType.Date => DateTemplate,
            FieldType.Boolean => BooleanTemplate,
            _ => null
        };
    }
}