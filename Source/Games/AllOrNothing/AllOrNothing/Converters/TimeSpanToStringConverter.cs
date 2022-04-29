using Microsoft.UI.Xaml.Data;
using System;

namespace AllOrNothing.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is TimeSpan))
                return null;
            var tmspn = (TimeSpan)value;
            return tmspn.ToString(@"mm\:ss"); //$"{tmspn.Minutes}:{tmspn.Seconds}";
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
