﻿using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;
using AllOrNothing.Mapping;
using AllOrNothing.Models;
using CommunityToolkit.WinUI.UI.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AllOrNothing.ViewModels
{
    public class ScoreBoardPageViewModel : ViewModelBase
    {
        public ScoreBoardPageViewModel(INavigationViewService navigationViewService)
            : base(navigationViewService)
        {

        }

        public void DataGridRowLoading(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex().ToString();
        }

        private ObservableCollection<StandingDto> _gameStandings;

        public ObservableCollection<StandingDto> GameStandings
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

        private string _roundDisplayText;
        public string RoundDisplayText
        {
            get => _roundDisplayText;
            set => SetProperty(ref _roundDisplayText, value);
        }
        public void InitVm(ObservableCollection<StandingDto> gameStandings)
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
    }
}