using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<Team, TeamDto>();
            
        }
    }
}
