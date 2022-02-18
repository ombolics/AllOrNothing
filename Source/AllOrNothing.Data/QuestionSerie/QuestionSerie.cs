using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Data
{
    public class QuestionSerie
    {
        [Key]
        public int Id { get; set; }
        public List<Topic> Topics { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}
