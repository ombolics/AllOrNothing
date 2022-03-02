using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(Team), ReverseMap = true)]
    public class TeamDto
    {
        public int Id { get; set; }
        public ObservableCollection<PlayerDto> Players 
        { 
            get;
            set;
        }
        private ICommand _addPlayerCommand;
        public ICommand AddPlayerCommand => _addPlayerCommand ??= new RelayCommand<PlayerDto>(DropPlayer);
        public event EventHandler<PlayerDto> PlayerDrop;
        public void DropPlayer(PlayerDto player)
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
