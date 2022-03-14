using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.ViewModels
{
    public class ScoreBoardPageViewModel : ObservableRecipient
    {
        public ScoreBoardPageViewModel()
        {

        }

        private IList<StandingDto> _lastGameStandings;

        public IList<StandingDto> LastGameStandings 
        { 
            get => _lastGameStandings;
            set => SetProperty(ref _lastGameStandings, value);
        }

        public void Setup(IList<StandingDto> lastGameStandings)
        {
            var ls = new List<StandingDto>(lastGameStandings);
            ls.Sort(new StandingDtoComparer());
            ls.Reverse();
            LastGameStandings = ls;
            
        }
    }
}
