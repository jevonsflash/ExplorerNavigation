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
    public class IsTypeToVisibilityConverter : IValueConverter
    {

        public Visibility NullValue { get; set; } = Visibility.Visible;
        public Visibility NonNullValue { get; set; } = Visibility.Collapsed;


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var target = Type.GetType(parameter as string);
            var result = (value.GetType() == target);
            return result ? NullValue : NonNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
