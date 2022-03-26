﻿using AllOrNothing.Data;
using AutoMapper;

namespace AllOrNothing.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            FromDataToDto();
            FromDtoToData();
        }

        private void ForomDtoToDto()
        {
            //CreateMap<QuestionSerieDto, QuestionSerieDto>();
            //CreateMap<TopicDto, TopicDto>();
            //CreateMap<QuestionSerieDto, QuestionSerieDto>();
        }
        private void FromDataToDto()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<Team, TeamDto>();

            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionSerie, QuestionSerieDto>();
            CreateMap<Topic, TopicDto>()
                .ForMember(dest => dest.OriginalName, opt => opt.MapFrom(src => src.Name));
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
