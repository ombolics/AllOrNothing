using AllOrNothing.Data;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(Player),ReverseMap = true)]
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Institue { get; set; }
        public string NickName { get; set; }

       // public ObservableCollection<PlayerDto> OriginalTeam { get; set; }

        //RelayCommand because interface types's serialisation is not allowed
        [Ignore]
        
        public RelayCommand<object> RemoveCommand { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
