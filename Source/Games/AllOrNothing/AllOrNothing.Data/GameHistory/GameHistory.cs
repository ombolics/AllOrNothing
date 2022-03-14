using System;
using System.Collections.Generic;

namespace AllOrNothing.Data
{
    class GameHistory
    {
        public int GameId { get; set; }
        public bool IsEnded { get; set; }
        public bool IsSuspended { get; set; }
        public List<Team> Teams { get; set; }
        public List<RoundHistory> Rounds { get; set; }
        public List<Standing> FinalStandings { get; set; }
        public DateTime Date { get; set; }
    }
}
