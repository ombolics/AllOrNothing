using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Models
{
    class AllOrNothingGameModel : ObservableRecipient
    {
        private int _numberOfRounds;

        public int NumberOfRounds 
        { 
            get => _numberOfRounds; 
            set => SetProperty(ref _numberOfRounds, value); 
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
            set => SetProperty(ref _maxTeamSize, value); 
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

        

        
    }
}
