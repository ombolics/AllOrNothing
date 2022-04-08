using AllOrNothing.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

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

            Binding foreGround = new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(ForeGround)),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.Default,
            };

            _display.SetBinding(TextBlock.TextProperty, b);
            _display.SetBinding(TextBlock.ForegroundProperty, foreGround);

            _display.Foreground = new SolidColorBrush((Color)App.Current.Resources["MainColor1"]);

            Border = new Border
            {
                //Background = new SolidColorBrush((Color)App.Current.Resources["MainColor2"]),
                CornerRadius = new CornerRadius(5),
            };

            Border.Child = Display;
            Children.Add(Border);
        }



        public Brush ForeGround
        {
            get { return (Brush)GetValue(ForeGroundProperty); }
            set { SetValue(ForeGroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForeGround.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForeGroundProperty =
            DependencyProperty.Register("ForeGround", typeof(Brush), typeof(PlayerTextBlock), null);



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
