using AllOrNothing.Data;
using AutoMapper;
using System.Collections.Generic;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(Topic), ReverseMap = true)]
    public class TopicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<QuestionDto> Questions { get; set; }
        public List<CompetenceDto> Competences { get; set; }
        public PlayerDto Author { get; set; }
    }
}
