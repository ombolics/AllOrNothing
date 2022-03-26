﻿using AllOrNothing.Mapping;
using AllOrNothing.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

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

            Children.Add(Display);
        }

        public Player Player
        {
            get { return (Player)GetValue(PlayerProperty); }
            set
            {
                SetValue(PlayerProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Player.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("Player", typeof(PlayerDto), typeof(PlayerTextBlock), null);


    }
}
