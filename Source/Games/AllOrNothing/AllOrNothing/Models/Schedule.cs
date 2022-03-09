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
    public class Schedule : ObservableRecipient
    {
        public Schedule()
        {
            _teams = new List<TeamDto>();
        }
        private QuestionSerieDto _serie;
        public QuestionSerieDto Serie 
        {
            get => _serie;
            set => SetProperty(ref _serie, value);
        }

        private List<TeamDto> _teams;
        public List<TeamDto> Teams 
        {
            get => _teams;
            set => SetProperty(ref _teams, value);
        }
    }
}
