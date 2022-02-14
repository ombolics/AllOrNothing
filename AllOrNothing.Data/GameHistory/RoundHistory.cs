using System;
using System.Collections.Generic;

namespace AllOrNothing.Data
{
    public class RoundHistory
    {
        public int Id { get; set; }
        public QuestionSerie QuestionSerie { get; set; }
        public List<Team> Teams { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Standing> FinalStandings { get; set; }
        public DateTime Date { get; set; }
        public int ActiveLenght { get; set; }
    }
}