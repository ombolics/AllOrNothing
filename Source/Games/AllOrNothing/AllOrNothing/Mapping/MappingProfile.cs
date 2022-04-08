using AllOrNothing.Data;
using AllOrNothing.Models;
using AutoMapper;

namespace AllOrNothing.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            FromDataToDto();
            FromDtoToData();
            ForomModelToModel();
        }

        private void ForomModelToModel()
        {
            CreateMap<PlayerDto, DragablePlayer>();
            CreateMap<DragablePlayer, PlayerDto>();
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
