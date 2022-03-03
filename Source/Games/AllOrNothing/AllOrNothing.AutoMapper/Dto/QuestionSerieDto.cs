using AllOrNothing.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(QuestionSerie), ReverseMap = true)]
    public class QuestionSerieDto
    {
        public int Id { get; set; }
        public List<TopicDto> Topics { get; set; }
        public List<PlayerDto> Authors { get; set; }
        public DateTime CreatedOn { get; set; }

        private HashSet<CompetenceDto> GetCompetences()
        {
            var value = new HashSet<CompetenceDto>();
            foreach (var topic in Topics)
            {
                foreach (var item in topic.Competences)
                {
                    value.Add(item);
                }
            }
            return value;
        }

        public HashSet<CompetenceDto> Competences => GetCompetences();
    }
}
