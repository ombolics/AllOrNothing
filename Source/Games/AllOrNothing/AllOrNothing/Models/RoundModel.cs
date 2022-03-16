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
