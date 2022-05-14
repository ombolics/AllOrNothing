using AllOrNothing.Contracts.Services;
using AllOrNothing.Mapping;
using AllOrNothing.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AllOrNothing.ViewModels
{
    public class ScoreBoardPageViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<StandingDto> _gameStandings;
        private IList<StandingDto> _lastRoundStandings;
        private string _roundDisplayText;
        #endregion

        #region Constructors
        public ScoreBoardPageViewModel(INavigationViewService navigationViewService)
            : base(navigationViewService)
        {
        }
        #endregion

        #region Properties
        public ObservableCollection<StandingDto> GameStandings
        {
            get => _gameStandings;
            set => SetProperty(ref _gameStandings, value);
        }
        public IList<StandingDto> LastRoundStandings
        {
            get => _lastRoundStandings;
            set => SetProperty(ref _lastRoundStandings, value);
        }
        public string RoundDisplayText
        {
            get => _roundDisplayText;
            set => SetProperty(ref _roundDisplayText, value);
        }
        #endregion

        #region Methods
        public void Init(ObservableCollection<StandingDto> gameStandings)
        {
            GameStandings = gameStandings;
            RoundDisplayText = "A legutóbbi kör eredménye";
        }

        public void UpdateStandings(RoundModel m)
        {
            //Sync the game standings with the last round's result
            LastRoundStandings = new List<StandingDto>(m.RoundStandings)
                .OrderByDescending(m => m.Score)
                .ToList();

            if (m.IsFinalRound)
            {
                RoundDisplayText = "A döntő eredménye";
                return;
            }

            foreach (var standing in LastRoundStandings)
            {
                var gameStanding = GameStandings.Single(s => s.Team == standing.Team);
                gameStanding.Score += standing.Score;
                gameStanding.MatchPlayed++;
            }
            GameStandings = new ObservableCollection<StandingDto>(GameStandings.OrderByDescending(s => s.Score).ThenBy(s => s.Team.TeamName).ToList());
        }
        #endregion
    }
}