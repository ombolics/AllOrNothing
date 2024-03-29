﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public class Topic
    {
        public Topic()
        {
            Questions = new List<Question>();
            Competences = new List<Competence>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Question> Questions { get; set; }
        public IList<Competence> Competences { get; set; }
        public Player Author { get; set; }
    }
}
