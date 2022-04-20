using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerNavigation.Model
{
    public enum ExplorerItemType { Folder, File };

    public class ExplorerItem : ObservableObject, IExplorerItem
    {
        public const char SpliterChar = '/';

        public string Name { get; set; }
        public string Path { get; set; }
        public List<string> PathStack => Path.Split(SpliterChar).ToList();

        private bool _isCurrent;

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                _isCurrent = value;

                OnPropertyChanged(nameof(IsCurrent));

            }
        }

        public ExplorerItemType Type { get; set; }
        private ObservableCollection<IExplorerItem> m_children;
        public ObservableCollection<IExplorerItem> Children
        {
            get
            {
                if (m_children == null)
                {
                    m_children = new ObservableCollection<IExplorerItem>();
                }
                return m_children;
            }
            set
            {
                m_children = value;
            }
        }

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                if (m_isExpanded != value)
                {
                    m_isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                }
            }
        }

        public override string ToString()
        {
            return this.Path;
        }
    }

}
