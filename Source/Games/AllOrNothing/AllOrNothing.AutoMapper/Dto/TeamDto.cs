using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(Team), ReverseMap = true)]
    public class TeamDto : ObservableRecipient
    {
        public int Id { get; set; }
        public ObservableCollection<Player> Players
        {
            get;
            set;
        }
        private ICommand _addPlayerCommand;
        public ICommand AddPlayerCommand => _addPlayerCommand ??= new RelayCommand<Player>(DropPlayer);
        public event EventHandler<Player> PlayerDrop;
        public void DropPlayer(Player player)
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
    }
}
