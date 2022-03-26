
using AllOrNothing.Mapping;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AllOrNothing.Models
{
    public class RoundSettingsModel : ObservableRecipient
    {
        public RoundSettingsModel(IList<TeamDto> teams, GameSettingsModel globalSettings)
        {
            Teams = teams;
            QuestionSerie = null;
            IsTematicalAllowed = globalSettings.IsTematicalAllowed;
            TematicalTime = globalSettings.GeneralTematicalTime;
            IsLightningAllowed = globalSettings.IsLightningAllowed;
            LightningTime = globalSettings.GeneralLightningTime;
            IsGameWithoutButtonsEnabled = globalSettings.IsGameWithoutButtonsEnabled;
        }
        public RoundSettingsModel()
        {

        }
        private IList<TeamDto> _teams;

        public IList<TeamDto> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }

        private QuestionSerieDto _questionSerie;
        public QuestionSerieDto QuestionSerie
        {
            get => _questionSerie;
            set => SetProperty(ref _questionSerie, value);
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

        private TimeSpan _tematicalTime;
        public TimeSpan TematicalTime
        {
            get => _tematicalTime;
            set => SetProperty(ref _tematicalTime, value);
        }

        private TimeSpan _lightningTime;
        public TimeSpan LightningTime
        {
            get => _lightningTime;
            set => SetProperty(ref _lightningTime, value);
        }

        private bool _isGameWithoutButtonsEnabled;
        public bool IsGameWithoutButtonsEnabled
        {
            get => _isGameWithoutButtonsEnabled;
            set => SetProperty(ref _isGameWithoutButtonsEnabled, value);
        }

        public static List<RoundSettingsModel> FromGameSettingsModel(GameSettingsModel m)
        {
            var value = new List<RoundSettingsModel>();
            foreach (var schedule in m.Schedules)
            {
                var roundSettings = new RoundSettingsModel
                {
                    Teams = schedule.Teams,
                    QuestionSerie = schedule.Serie,
                    IsTematicalAllowed = m.IsTematicalAllowed,
                    TematicalTime = m.GeneralTematicalTime,
                    IsLightningAllowed = m.IsLightningAllowed,
                    LightningTime = m.GeneralLightningTime,
                    IsGameWithoutButtonsEnabled = m.IsGameWithoutButtonsEnabled,
                };

                value.Add(roundSettings);
            }
            return value;
        }
    }
}
