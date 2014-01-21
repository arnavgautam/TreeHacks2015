using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EventBuddy.Common
{
    public sealed class ItemCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int? itemCount = value as int?;

            return (itemCount.HasValue && itemCount > 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string itemCount)
        {
            return Convert(value, targetType, parameter, itemCount);
        }
    }
}
