using AllOrNothing.Mapping;
using AllOrNothing.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using AllOrNothing.Models;
using Microsoft.UI.Xaml.Media;

namespace AllOrNothing.Controls
{
    class PlayerTextBlock : StackPanel
    {
        private TextBlock _display;
        public TextBlock Display
        {
            get => _display;
            set => _display = value;
        }
        private Border _border;
        public Border Border
        {
            get => _border;
            set => _border = value;
        }
        public PlayerTextBlock()
        {
            _display = new TextBlock();
            Binding b = new Binding
            {
                Source = this,
                Path = new PropertyPath($"{nameof(Player)}.{nameof(Player.Name)}"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            _display.SetBinding(TextBlock.TextProperty, b);

            Border = new Border
            {
                Background = (Brush)App.Current.Resources["GreyBrush"],
                CornerRadius = new CornerRadius(10),
            };

            Border.Child = Display;
            Children.Add(Border);
        }

        public DragablePlayer Player
        {
            get { return (DragablePlayer)GetValue(PlayerProperty); }
            set
            {
                SetValue(PlayerProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Player.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("Player", typeof(DragablePlayer), typeof(PlayerTextBlock), null);


    }
}
