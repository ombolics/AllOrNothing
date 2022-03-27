using AllOrNothing.Data;
using AllOrNothing.Models;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Team), ReverseMap = true)]
    public class TeamDto : ObservableRecipient
    {
        public int Id { get; set; }
        public ObservableCollection<DragablePlayer> Players
        {
            get;
            set;
        }
        private ICommand _addPlayerCommand;
        public ICommand AddPlayerCommand => _addPlayerCommand ??= new RelayCommand<DragablePlayer>(DropPlayer);
        public event EventHandler<DragablePlayer> PlayerDrop;
        public void DropPlayer(DragablePlayer player)
        {

            PlayerDrop?.Invoke(this, player);

            //Almost good
            //player.OriginalTeam.Remove(player);
            //Players.Add(player);
            //player.OriginalTeam = Players;
        }

        public void UpdateTeamName()
        {        
            if (Players.Count <= 0)
                return;

            TeamName = "";
            for (int i = 0; i < Players.Count; i++)
            {
                TeamName += i == Players.Count - 1 ? Players[i].NickName : Players[i].NickName + " - ";
            }
        }

        private string _teamName;
        public string TeamName 
        {
            get => _teamName;
            set => SetProperty(ref _teamName, value);
        }

        private int _minPlayerCount;
        private int _maxPlayerCount;
        private bool _canDragPlayers;
        private bool _allowDrop;

        internal void CheckGuards(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CanDragPlayers = _minPlayerCount < Players.Count;
            AllowDrop = _maxPlayerCount > Players.Count;
            foreach (var item in Players)
            {
                //PLayer?????
                item.CanDrag = CanDragPlayers;
            }
        }


        public bool CanDragPlayers
        {
            get => _canDragPlayers;
            set => SetProperty(ref _canDragPlayers, value);
        }
        public bool AllowDrop
        {
            get => _allowDrop;
            set => SetProperty(ref _allowDrop, value);
        }
        public int MinPlayerCount
        {
            get => _minPlayerCount;
            set => SetProperty(ref _minPlayerCount, value);
        }
        public int MaxPlayerCount
        {
            get => _maxPlayerCount;
            set => SetProperty(ref _maxPlayerCount, value);
        }
    }
}
