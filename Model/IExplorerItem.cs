using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExplorerNavigation.Model
{
    public interface IExplorerItem
    {
        ObservableCollection<IExplorerItem> Children { get; set; }
        bool IsCurrent { get; set; }
        bool IsExpanded { get; set; }
        string Name { get; set; }
        string Path { get; set; }
        List<string> PathStack { get; }
        ExplorerItemType Type { get; set; }
    }
}