using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(Team), ReverseMap = true)]
    public class TeamDto
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
        public string TeamName { get; set; }
    }
}
