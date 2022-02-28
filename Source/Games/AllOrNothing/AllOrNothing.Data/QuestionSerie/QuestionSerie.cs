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
        public List<Player> Authors { get; set; }
        public DateTime CreatedOn { get; set; }

        private HashSet<Competence> GetCompetences()
        {
            var value = new HashSet<Competence>();
            foreach (var topic in Topics)
            {
                foreach (var item in topic.Competences)
                {
                    value.Add(item);
                }
            }
            return value;
        }

        public HashSet<Competence> Competences => GetCompetences();
    }
}
