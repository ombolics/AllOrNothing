using AllOrNothing.Helpers;
using AllOrNothing.Mapping;
using AllOrNothing.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using System.Collections.Generic;
using System.Linq;

namespace AllOrNothing.ViewModels
{
    public class ScoreBoardPageViewModel : ObservableRecipient
    {
        public ScoreBoardPageViewModel()
        {

        }

        public void DataGridRowLoading(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex().ToString();
        }

        private List<StandingDto> _gameStandings;

        public List<StandingDto> GameStandings
        {
            get => _gameStandings;
            set => SetProperty(ref _gameStandings, value);
        }

        private IList<StandingDto> _lastRoundStandings;

        public IList<StandingDto> LastRoundStandings
        {
            get => _lastRoundStandings;
            set => SetProperty(ref _lastRoundStandings, value);
        }

        public void Setup(RoundModel m)
        {
            var ls = new List<StandingDto>(m.RoundStandings);
            ls.Sort(new StandingDtoComparer());
            ls.Reverse();
            LastRoundStandings = ls;

            //Sync the game standings with the last round's result


            //TODO optimize this
            foreach (var teamInStandings in GameStandings)
            {
                foreach (var teamFromModel in m.RoundStandings)
                {
                    if (teamInStandings.Team.Players.Select(x => x.Id).SequenceEqual(teamFromModel.Team.Players.Select(x => x.Id)))
                    {
                        teamInStandings.Score += teamFromModel.Score;
                        teamInStandings.MatchPlayed++;
                    }
                }
            }

            GameStandings.Sort(new StandingDtoComparer());
            GameStandings.Reverse();

        }
    }
}