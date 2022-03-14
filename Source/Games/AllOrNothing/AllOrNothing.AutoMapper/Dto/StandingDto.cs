﻿using CommunityToolkit.Mvvm.ComponentModel;

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
    }
}
