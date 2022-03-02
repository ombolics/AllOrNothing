using AllOrNothing.AutoMapper.Dto;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Path = new PropertyPath(nameof(Player)),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            _display.SetBinding(TextBlock.TextProperty,b);

            Children.Add(Display);
        }

        public PlayerDto Player
        {
            get { return (PlayerDto)GetValue(PlayerProperty); }
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
