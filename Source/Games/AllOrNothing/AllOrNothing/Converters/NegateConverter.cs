using Microsoft.UI.Xaml.Data;
using System;

namespace AllOrNothing.Converters
{
    public class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool original)
                return !original;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
