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
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<Topic> Topics 
        {
            get; 
            set;
        }

        public DateTime CreatedOn 
        { 
            get;
            set;
        }     
    }
}
