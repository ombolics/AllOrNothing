
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public List<Competence> Competences { get; set; }
        public Player Author { get; set; }
    }
}
