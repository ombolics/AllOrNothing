using AllOrNothing.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Converters
{
    public class GamePhaseToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !(value is GamePhase))
                return Visibility.Collapsed;

            var gP = (GamePhase)value;
            string param = parameter as string;

            if((param == "Tematical" && gP == GamePhase.TEMATICAL) ||
                (param == "Lightning" && gP == GamePhase.LIGHTNING))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
