using AllOrNothing.Mapping;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Models
{
    public class GameModel : ObservableRecipient
    {
        public GameModel()
        {

        }
        public GameModel(GameSettingsModel settings, ObservableCollection<StandingDto> standings)
        {
            GameSettings = settings;
            GameStandings = standings;
        }
        private ObservableCollection<StandingDto> _gameStandings;
        public ObservableCollection<StandingDto> GameStandings
        {
            get => _gameStandings;
            set => SetProperty(ref _gameStandings, value);
        }

        private GameSettingsModel _gameSettings;

        public GameSettingsModel GameSettings
        {
            get => _gameSettings;
            set => SetProperty(ref _gameSettings, value);
        }

       
    }
}
