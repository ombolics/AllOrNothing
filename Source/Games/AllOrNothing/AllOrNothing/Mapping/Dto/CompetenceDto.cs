using AllOrNothing.Data;
using AutoMapper;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Competence), ReverseMap = true)]
    public class CompetenceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}