using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Data;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOrNothing.Controls
{
    class TeamStackPanel : StackPanel
    {
        public event EventHandler<PlayerDto> PlayerDropped;

        private ICommand _playerDroppedCommand;

        public ICommand PlayerDroppedCommand => _playerDroppedCommand ??= new RelayCommand<PlayerDto>(On_MemberDropped);

        public void TestHandler(object sender, PlayerDto p)
        {

        }

        public TeamDto Team
        {
            get { return (TeamDto)GetValue(TeamProperty); }
            set { SetValue(TeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TeamProperty =
            DependencyProperty.Register("Team", typeof(TeamDto), typeof(TeamStackPanel), null);



        private void On_MemberDropped(PlayerDto player)
        {
            PlayerDropped?.Invoke(this, player);
        }
    }
}
