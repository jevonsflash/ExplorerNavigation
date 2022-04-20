using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExplorerNavigation.Common;
using ExplorerNavigation.Model;
using System.Collections.Generic;
using System.Linq;

namespace ExplorerNavigation.ViewModel
{
    public class ExplorerViewModel : ObservableObject
    {
        public RelayCommand NavigationHistoryBackCommand { get; private set; }
        public RelayCommand NavigationHistoryForwardCommand { get; private set; }
        public RelayCommand NavigationBackCommand { get; private set; }
        public ExplorerViewModel()
        {
            NavigationHistoryBackCommand = new RelayCommand(NavigationHistoryBack);
            NavigationHistoryForwardCommand = new RelayCommand(NavigationHistoryForward);
            NavigationBackCommand = new RelayCommand(NavigationBack);

            NavigationStack = new ObservableCollectionEx<IExplorerItem>();
            NavigationHistoryStack = new ObservableCollectionEx<IExplorerItem>();
        }

        public virtual void ClearStack()
        {
            NavigationStack.Clear();
            NavigationHistoryStack.Clear();
        }

        private void NavigationBack()
        {
            if (NavigationStack.Count == 1)
            {
                return;
            }
            NavigationStack.Pop();
            var lastItem = NavigationStack.LastOrDefault();
            if (lastItem == null)
            {
                return;
            }
            if (ToFolder(lastItem))
            {

                NavigationHistoryStack.ForEach((element) =>
                {
                    element.IsCurrent = false;
                });
                lastItem.IsCurrent = true;

                PushNavigationHistoryStack(lastItem);
            }
        }
        private void PushNavigationHistoryStack(IExplorerItem item)
        {
            var newItem = new ExplorerItem
            {
                Name = item.Name,
                Path = item.Path,
                IsCurrent = item.IsCurrent,
                Type = item.Type,
                Children = item.Children,
                IsExpanded = false
            };

            if (NavigationHistoryStack.Count > 10)
            {
                NavigationHistoryStack.Pop();
            }
            NavigationHistoryStack.Unshift(newItem);
        }

        private void DealWithNavigationStack(IExplorerItem folder)
        {

            NavigationStack.Clear();
            var paths = folder.Path.Split(ExplorerItem.SpliterChar);


            void a(IEnumerable<IExplorerItem> ex, int index)
            {
                if (index > paths.Length - 1)
                {
                    return;
                }

                var currentName = paths[index];
                var currentExplorerItem = ex.FirstOrDefault(c => c.Name == currentName);
                if (currentExplorerItem == null)
                {
                    return;

                }
                NavigationStack.Push(currentExplorerItem);
                a(currentExplorerItem.Children, index + 1);
            }
            a(RootExplorerItems, 0);
        }

        public virtual void NavigationTo(IExplorerItem folder)
        {
            DealWithNavigationStack(folder);
            if (ToFolder(folder))
            {
                NavigationHistoryStack.ForEach((element) =>
                {
                    element.IsCurrent = false;
                });
                folder.IsCurrent = true;
                PushNavigationHistoryStack(folder);
            }
        }

        private void NavigationHistoryBack()
        {
            var currentIndex = NavigationHistoryStack.IndexOf(
              (c) => c.IsCurrent
            );
            if (currentIndex < NavigationHistoryStack.Count - 1)
            {
                var forwardIndex = currentIndex + 1;

                var folder = NavigationHistoryStack[forwardIndex];
                DealWithNavigationStack(folder);

                if (ToFolder(folder))
                {
                    NavigationHistoryStack.ForEach((element) =>
                    {
                        element.IsCurrent = false;
                    });
                    NavigationHistoryStack[forwardIndex].IsCurrent = true;
                }
            }
        }

        private void NavigationHistoryForward()
        {

            var currentIndex = NavigationHistoryStack.IndexOf(
              (c) => c.IsCurrent
            );
            if (currentIndex > 0)
            {
                var forwardIndex = currentIndex - 1;

                var folder = NavigationHistoryStack[forwardIndex];
                DealWithNavigationStack(folder);

                if (ToFolder(folder))
                {
                    NavigationHistoryStack.ForEach((element) =>
                    {
                        element.IsCurrent = false;
                    });
                    NavigationHistoryStack[forwardIndex].IsCurrent = true;
                }
            }
        }

        public virtual bool ToFolder(IExplorerItem item)
        {
            if (item == null || item.Path == CurrentExplorerItem.Path)
            {
                return false;
            }

            var currentExplorerItem = NavigationStack.FirstOrDefault(c => c.Path == item.Path);

            if (currentExplorerItem == null)
            {
                return false;
            }

            CurrentExplorerItem = currentExplorerItem;
            return true;
        }


        private ObservableCollectionEx<IExplorerItem> _navigationStack;

        public ObservableCollectionEx<IExplorerItem> NavigationStack
        {
            get { return _navigationStack; }
            set
            {
                _navigationStack = value;
                OnPropertyChanged(nameof(NavigationStack));
            }
        }

        private ObservableCollectionEx<IExplorerItem> _navigationHistoryStack;

        public ObservableCollectionEx<IExplorerItem> NavigationHistoryStack
        {
            get { return _navigationHistoryStack; }
            set
            {
                _navigationHistoryStack = value;
                OnPropertyChanged(nameof(NavigationHistoryStack));
            }
        }

        private IExplorerItem GenerateExplorerItem(IExplorerItem item)
        {
            var paths = item.Path.Split(ExplorerItem.SpliterChar);

            IExplorerItem a(IEnumerable<IExplorerItem> ex, int index)
            {
                if (index > paths.Length - 1)
                {
                    return null;
                }

                var currentName = paths[index];
                var currentExplorerItem = ex.FirstOrDefault(c => c.Name == currentName);
                if (currentExplorerItem == null)
                {
                    return null;
                }
                return a(currentExplorerItem.Children, index + 1);
            }
            var currentExplorerItem = a(this.RootExplorerItems, 0);
            return currentExplorerItem;
        }
        public virtual bool GetIsCurrentHistoryNavigationItem(IExplorerItem item)
        {
            var result = item.IsCurrent;
            return result;
        }

        private ObservableCollectionEx<IExplorerItem> _rootExplorerItem;

        public ObservableCollectionEx<IExplorerItem> RootExplorerItems
        {
            get { return _rootExplorerItem; }
            set
            {
                _rootExplorerItem = value;
                OnPropertyChanged(nameof(RootExplorerItems));
            }
        }

        private IExplorerItem _currentExplorerItem;

        public IExplorerItem CurrentExplorerItem
        {
            get { return _currentExplorerItem; }
            set
            {
                _currentExplorerItem = value;
                OnPropertyChanged(nameof(CurrentExplorerItem));

            }
        }

    }
}