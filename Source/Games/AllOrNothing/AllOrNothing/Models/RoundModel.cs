using AllOrNothing.AutoMapper.Dto;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Models
{
    public class RoundModel : ObservableRecipient
    {
        public RoundModel(RoundSettingsModel settings, ObservableCollection<StandingDto> standings)
        {
            RoundSettings = settings;
            RoundStandings = standings;
        }

        /// <summary>
        /// Creates a default instance, with a given collection of teams
        /// </summary>
        /// <param name="teams"> Collection of teams</param>
        public RoundModel(IList<TeamDto> teams, GameSettingsModel globalSettings)
        {
            RoundStandings = new ObservableCollection<StandingDto>(StandingDto.DefaultStaningsFromTeams(teams));
            RoundSettings = new RoundSettingsModel(teams, globalSettings);
        }

        private ObservableCollection<StandingDto> _roundStandings;
        public ObservableCollection<StandingDto> RoundStandings
        { 
            get => _roundStandings; 
            set => SetProperty(ref _roundStandings, value); 
        }

        private RoundSettingsModel _roundSettings;

        public RoundSettingsModel RoundSettings 
        { 
            get => _roundSettings;
            set => SetProperty(ref _roundSettings, value);
        }

        private bool _roundEnded;
        public bool RoundEnded
        {
            get => _roundEnded;
            set => SetProperty(ref _roundEnded, value);
        }

        private bool _isFinalRound;
        public bool IsFinalRound
        {
            get => _isFinalRound;
            set => SetProperty(ref _isFinalRound, value);
        }
        public static List<RoundModel> FromGameModel(GameModel gameModel)
        {
            List<RoundModel> collection = new List<RoundModel>();
            var roundSettings = RoundSettingsModel.FromGameSettingsModel(gameModel.GameSettings);

            foreach (var item in roundSettings)
            {
                var standings = new ObservableCollection<StandingDto>();
                foreach (var team in item.Teams)
                {
                    standings.Add(new StandingDto
                    {
                        Team = team,
                        Score = 0,
                        MatchPlayed = 0,
                    });
                }
                collection.Add(new RoundModel(item, standings));
            }
            return collection;
        }

    }
}
