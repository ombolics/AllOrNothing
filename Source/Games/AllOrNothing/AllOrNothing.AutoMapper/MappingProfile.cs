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
            FromDataToDto();
            FromDtoToData();
        }

        private void FromDataToDto()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<Team, TeamDto>();

            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionSerie, QuestionSerieDto>();
            CreateMap<Topic, TopicDto>();
            CreateMap<Competence, CompetenceDto>();
        }

        private void FromDtoToData()
        {
            CreateMap<PlayerDto, Player>();
            CreateMap<TeamDto, Team>();

            CreateMap<QuestionDto, Question>();
            CreateMap<QuestionSerieDto, QuestionSerie>();
            CreateMap<TopicDto, Topic>();
            CreateMap<CompetenceDto, Competence>();
        }
    }
}
