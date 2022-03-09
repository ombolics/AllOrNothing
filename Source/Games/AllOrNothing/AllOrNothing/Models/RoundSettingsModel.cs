
using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Models
{
    public class RoundSettingsModel : ObservableRecipient
    {
        private List<TeamDto> _teams;

        public List<TeamDto> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }

        public QuestionSerieDto QuestionSerie { get; set; }
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
                };

                value.Add(roundSettings);
            }
            return value;
        }
    }
}
