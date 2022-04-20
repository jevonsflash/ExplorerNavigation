using CommunityToolkit.Mvvm.Input;
using ExplorerNavigation.Common;
using ExplorerNavigation.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace ExplorerNavigation.ViewModel
{
    public class MainPageViewModel : ExplorerViewModel
    {

        //文件目录结构
        private ExplorerItem root = new ExplorerItem()
        {
            Children = new ObservableCollection<IExplorerItem>() {
                    new ExplorerItem() {
                        Children = new ObservableCollection<IExplorerItem>() {

                            new ExplorerItem() {
                                Name = "文件夹B",
                                Path = "我的网盘/文件夹A/文件夹B",
                                Type = ExplorerItemType.Folder

                             },
                            new ExplorerItem() {
                                Children = new ObservableCollection<IExplorerItem>() {

                                    new ExplorerItem() {
                                        Name = "文件夹D",
                                        Path = "我的网盘/文件夹A/文件夹C/文件夹D",
                                        Type = ExplorerItemType.Folder

                                     },
                                },
                                Name = "文件夹C",
                                Path = "我的网盘/文件夹A/文件夹C",
                                Type = ExplorerItemType.Folder

                             },
                            new ExplorerItem() {

                                Name = "文件3",
                                Path = "我的网盘/文件夹A/文件3",
                                Type = ExplorerItemType.File

                            }

                        },
                        Name = "文件夹A",
                        Path = "我的网盘/文件夹A",
                        Type = ExplorerItemType.Folder

                    },
                    new ExplorerItem() {

                        Name = "文件1",
                        Path = "我的网盘/文件1",
                        Type = ExplorerItemType.File

                    },
                    new ExplorerItem() {

                        Name = "文件2",
                        Path = "我的网盘/文件2",
                        Type = ExplorerItemType.File

                    }
                },
            Name = "我的网盘",
            Path = "我的网盘",
            Type = ExplorerItemType.Folder
        };


        public MainPageViewModel()
        {
            PropertyChanged += MenuPageViewModel_PropertyChanged;
            //init data
            PathStack = new ObservableCollection<string>();
            this.CurrentFileInfos = new ObservableCollectionEx<IFileInfo>();
            InitData();
        }

        /// <summary>
        /// 订阅导航的跳转变动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MenuPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentExplorerItem))
            {
                PathStack.Clear();

                if (CurrentExplorerItem == null || string.IsNullOrEmpty(CurrentExplorerItem.Path))
                {
                    return;
                }

                foreach (var item in CurrentExplorerItem.PathStack)
                {
                    PathStack.Add(item);
                }

                this.CurrentFileInfos.Clear();
                foreach (var item in CurrentExplorerItem.Children)
                {
                    this.CurrentFileInfos.Add(new ExplorerNavigation.Model.FileInfo()
                    {
                        FileName = item.Name,
                        FileSize = "1000kb",
                        FileType = ".file",
                        Type = item.Type is ExplorerItemType.Folder ? FileInfoType.Folder : FileInfoType.File,
                        Path = item.Path,
                    });
                }



                SelectedFileInfo = null;
            }

        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        private async void InitData()
        {
            RootExplorerItems = new ObservableCollectionEx<IExplorerItem>() { root };

            NavigationHistoryStack.Add(root);
            NavigationStack.Add(root);

            CurrentExplorerItem = RootExplorerItems.FirstOrDefault();
        }




        private ObservableCollectionEx<IFileInfo> _currentFileInfos;

        public ObservableCollectionEx<IFileInfo> CurrentFileInfos
        {
            get { return _currentFileInfos; }
            set
            {
                _currentFileInfos = value;
                OnPropertyChanged(nameof(CurrentFileInfos));
            }
        }


        private IFileInfo _selectedFileInfo;

        public IFileInfo SelectedFileInfo
        {
            get { return _selectedFileInfo; }
            set
            {
                _selectedFileInfo = value;
                OnPropertyChanged(nameof(SelectedFileInfo));
            }
        }


        private ObservableCollection<string> _pathStack;

        public ObservableCollection<string> PathStack
        {
            get { return _pathStack; }
            set
            {
                _pathStack = value;
                OnPropertyChanged(nameof(PathStack));

            }
        }

    }
}
