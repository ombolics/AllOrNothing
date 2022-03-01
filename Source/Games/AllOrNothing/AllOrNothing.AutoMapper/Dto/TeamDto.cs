using AllOrNothing.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
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
        public List<PlayerDto> Players { get; set; }
        public ICommand TestCommand { get; set; }
        public string TeamName { get; set; }
    }
}
