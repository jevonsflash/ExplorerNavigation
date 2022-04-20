using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace ExplorerNavigation.Model
{
    public class FileInfo : IFileInfo
    {
        public const string FolderType = "application/local-object-manager";

        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }

        public string CreateDate { get; set; }

        public string Path { get; set; }

        public int Type { get; set; }

    }
}
