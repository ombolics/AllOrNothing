using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace AllOrNothing.AutoMapper.Dto
{
    public class StandingDto : ObservableRecipient

    {
        public int Id { get; set; }
        private TeamDto _team;
        public TeamDto Team
        {
            get => _team;
            set => SetProperty(ref _team, value);
        }
        private int _score;
        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }
        public int MatchPlayed 
        { 
            get => _matchPlayed; 
            set => SetProperty(ref _matchPlayed, value); 
        }

        private int _matchPlayed;

        public static ICollection<StandingDto> DefaultStaningsFromTeams(ICollection<TeamDto> teams)
        {
            var value = new List<StandingDto>();
            foreach (var item in teams)
            {
                value.Add(new StandingDto
                {
                    MatchPlayed = 0,
                    Team = item,
                    Score = 0,
                }) ;
            }
            return value;
        }
    }
}
