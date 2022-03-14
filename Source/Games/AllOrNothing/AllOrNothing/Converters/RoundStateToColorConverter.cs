using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace AllOrNothing.Converters
{
    class RoundStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is bool))
                throw new ArgumentException();
            var boolValue = (bool)value;

            var brush = new SolidColorBrush();
            if (boolValue)
            {
                brush.Color = Color.FromArgb(255, 46, 95, 105);
            }
            else
            {
                brush.Color = Color.FromArgb(255, 184, 220, 227);
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
