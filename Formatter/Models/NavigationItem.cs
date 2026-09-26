namespace Formatter.Models;

public class NavigationItem
{
    public required string Title { get; set; }
    
    public required Type TargetPageType { get; set; }
}
