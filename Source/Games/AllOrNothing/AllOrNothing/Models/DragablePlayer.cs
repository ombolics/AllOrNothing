﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOrNothing.Models
{
    public class DragablePlayer : ObservableRecipient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Institute { get; set; }
        public string NickName { get; set; }
        public bool IsDeleted { get; set; }
        private bool _canDrag;
        public bool CanDrag 
        {
            get => _canDrag;
            set => SetProperty(ref _canDrag, value);
        }

        private ICommand _changePlayerCommand;
        public ICommand ChangePlayerCommand => _changePlayerCommand ??= new RelayCommand<DragablePlayer>(On_ChangePlayer);
        public event EventHandler<DragablePlayer> SwitchPlayers;

        private void On_ChangePlayer(DragablePlayer obj)
        {
            SwitchPlayers?.Invoke(this, obj);
        }
    }
}
