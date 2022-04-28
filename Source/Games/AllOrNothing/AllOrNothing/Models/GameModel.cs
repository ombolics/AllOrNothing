using AllOrNothing.Mapping;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AllOrNothing.Models
{
    public class GameModel : ObservableRecipient
    {
        private GameSettingsModel _gameSettings;
        private ObservableCollection<StandingDto> _gameStandings;

        public GameModel()
        {

        }
        public GameModel(GameSettingsModel settings, ObservableCollection<StandingDto> standings)
        {
            GameSettings = settings;
            GameStandings = standings;
        }
        
        public ObservableCollection<StandingDto> GameStandings
        {
            get => _gameStandings;
            set => SetProperty(ref _gameStandings, value);
        }

        public GameSettingsModel GameSettings
        {
            get => _gameSettings;
            set => SetProperty(ref _gameSettings, value);
        }
    }
}
