using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace AllOrNothing.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is bool))
                throw new ArgumentException();

            var param = parameter?.ToString().ToLower();
            if(parameter == null)
            {
                bool objValue = (bool)value;
                if (objValue)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            else
            {
                bool objValue = (bool)value;
                if (!objValue)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }
}
