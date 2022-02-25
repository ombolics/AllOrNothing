using AllOrNothing.Data;
using CommunityToolkit.Mvvm.Input;
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
        public event EventHandler<Player> PlayerDropped;

        private ICommand _playerDroppedCommand;

        public ICommand PlayerDroppedCommand => _playerDroppedCommand ??= new RelayCommand<Player>(On_MemberDropped);

        private void On_MemberDropped(Player p)
        {
            PlayerDropped?.Invoke(this, p);
        }
    }
}
