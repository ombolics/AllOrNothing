using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public class QuestionSerie
    {
        public QuestionSerie()
        {
            Topics = new List<Topic>();
            Authors = new List<Player>();
            Competences = new HashSet<Competence>();
        }
        [Key]
        public int Id { get; set; }
        public ICollection<Topic> Topics 
        {
            get; 
            set;
        }
        public ICollection<Player> Authors { get; set; }
        public DateTime CreatedOn 
        { 
            get;
            set;
        }     

        public ICollection<Competence> Competences { get; set; }
    }
}
