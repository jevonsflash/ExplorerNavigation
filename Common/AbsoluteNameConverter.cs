using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerNavigation.Common
{
    public class AbsoluteNameConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var fileName = value as string;
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;

            }
            if (fileName.EndsWith('/') && fileName.Length > 1)
            {
                fileName = fileName.Substring(0, fileName.Length - 1);
            }
            var result = Path.GetFileName(fileName);
            return  string.IsNullOrEmpty(result) ? value : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
