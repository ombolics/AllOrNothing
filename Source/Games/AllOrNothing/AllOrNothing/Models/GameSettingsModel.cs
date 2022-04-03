using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AllOrNothing.Models
{
    public class GameSettingsModel : ObservableRecipient
    {
        public GameSettingsModel()
        {
            MaxTeamSize = 2;
            NumberOfRounds = 1;
        }

        private List<Schedule> _schedules;

        public List<Schedule> Schedules
        {
            get { return _schedules; }
            set => SetProperty(ref _schedules, value);
        }


        private string _occasionName;
        public string OccasionName
        {
            get => _occasionName;
            set => SetProperty(ref _occasionName, value);
        }

        private int _numberOfRounds;

        public int NumberOfRounds
        {
            get => _numberOfRounds;
            set
            {              
                if(value != _numberOfRounds)
                {
                   _numberOfRounds = value < 1 ? 1 : value;
                   OnPropertyChanged(nameof(NumberOfRounds));
                }              
            }
        }

        private bool _generateSchedule;
        public bool GenerateSchedule
        {
            get => _generateSchedule;
            set => SetProperty(ref _generateSchedule, value);
        }

        private bool _assignSeriesForAllGame;
        public bool AssignSeriesForAllGame
        {
            get => _assignSeriesForAllGame;
            set => SetProperty(ref _assignSeriesForAllGame, value);
        }

        private bool _teamsAllowed;
        public bool TeamsAllowed
        {
            get => _teamsAllowed;
            set => SetProperty(ref _teamsAllowed, value);
        }

        private int _maxTeamSize;
        public int MaxTeamSize
        {
            get => _maxTeamSize;
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _maxTeamSize = value;
                OnPropertyChanged(nameof(MaxTeamSize));
                //SetProperty(ref _maxTeamSize, value);
            }
        }
        private bool _generateTeams;
        public bool GenerateTeams
        {
            get => _generateTeams;
            set => SetProperty(ref _generateTeams, value);
        }

        private bool _isTematicalAllowed;
        public bool IsTematicalAllowed
        {
            get => _isTematicalAllowed;
            set => SetProperty(ref _isTematicalAllowed, value);
        }

        private bool _isLightningAllowed;
        public bool IsLightningAllowed
        {
            get => _isLightningAllowed;
            set => SetProperty(ref _isLightningAllowed, value);
        }

        private TimeSpan _generalTematicalTime;
        public TimeSpan GeneralTematicalTime
        {
            get => _generalTematicalTime;
            set => SetProperty(ref _generalTematicalTime, value);
        }

        private TimeSpan _generalLightningTime;
        public TimeSpan GeneralLightningTime
        {
            get => _generalLightningTime;
            set => SetProperty(ref _generalLightningTime, value);
        }

        private bool _isGameWithoutButtonsEnabled;
        public bool IsGameWithoutButtonsEnabled
        {
            get => _isGameWithoutButtonsEnabled;
            set => SetProperty(ref _isGameWithoutButtonsEnabled, value);
        }

        private bool _gameEnded;
        public bool GameEnded
        {
            get => _gameEnded;
            set => SetProperty(ref _gameEnded, value);
        }
    }
}
