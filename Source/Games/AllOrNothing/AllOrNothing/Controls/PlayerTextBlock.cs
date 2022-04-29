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
        #region Fields
        private TextBlock _display;
        private Border _border;
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
        public TextBlock Display
        {
            get => _display;
            set => _display = value;
        }
        public Border Border
        {
            get => _border;
            set => _border = value;
        }
        public Brush ForeGround
        {
            get { return (Brush)GetValue(ForeGroundProperty); }
            set { SetValue(ForeGroundProperty, value); }
        }
        public DragablePlayer Player
        {
            get { return (DragablePlayer)GetValue(PlayerProperty); }
            set
            {
                SetValue(PlayerProperty, value);
            }
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty ForeGroundProperty =
            DependencyProperty.Register("ForeGround", typeof(Brush), typeof(PlayerTextBlock), null);

        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("Player", typeof(DragablePlayer), typeof(PlayerTextBlock), null);
        #endregion
    }
}
