using Microsoft.UI.Xaml.Data;
using System;

namespace AllOrNothing.Converters
{
    public class NullValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                return value;

            string str = (string)value;
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
