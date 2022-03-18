using AllOrNothing.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(QuestionSerie), ReverseMap = true)]
    public class QuestionSerieDto
    {
        public int Id { get; set; }
        public List<TopicDto> Topics { get; set; }
        public HashSet<PlayerDto> Authors 
        {
            get => GetAuthors();
        }
        public bool FromFile { get; set; }
        private HashSet<PlayerDto> GetAuthors()
        {
            var value = new HashSet<PlayerDto>();
            var ids = new List<int>();
            foreach (var item in Topics)
            {
                if(item.Author != null && !ids.Contains(item.Author.Id))
                {
                    ids.Add(item.Author.Id);
                    value.Add(item.Author);
                }
                
            }
            return value;
        }

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

        public HashSet<CompetenceDto> Competences
        {
            get => GetCompetences();
        }
    }
}
