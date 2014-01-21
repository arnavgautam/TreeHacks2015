using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace EventBuddy.Common
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {            
            if (parameter == null)            
                return value;            

            return String.Format((String)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            DateTime temp;
            if (DateTime.TryParse(value.ToString(), out temp))
            {
                return temp;
            }

            return value;
        }
    }

}
