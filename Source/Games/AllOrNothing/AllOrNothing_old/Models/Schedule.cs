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
            _teams = new List<Team>();
        }
        private QuestionSerie _serie;
        public QuestionSerie Serie 
        {
            get => _serie;
            set => SetProperty(ref _serie, value);
        }

        private List<Team> _teams;
        public List<Team> Teams 
        {
            get => _teams;
            set => SetProperty(ref _teams, value);
        }
    }
}
