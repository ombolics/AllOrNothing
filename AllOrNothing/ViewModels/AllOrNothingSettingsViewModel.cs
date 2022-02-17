using AllOrNothing.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingSettingsViewModel : ObservableRecipient
    {

        public AllOrNothingSettingsViewModel()
        {
            _gameSettingsVisible = Visibility.Visible;
            _roundSettingsVisible = Visibility.Collapsed;
            _listViewItemSource = new ObservableCollection<Team>();
            _playerTest = new ObservableCollection<Player>();

            for (int i = 0; i < 10; i++)
            {
                _listViewItemSource.Add(
                     new Team
                     {
                         Players = new List<Player>
                         {
                                new Player
                                {
                                    Institue = "Bonyhád",
                                    Name = "Z. András",
                                    NickNames =new List<string> {"Bandi", "Zarkó"}
                                },
                                new Player
                                {
                                    Institue = "Bonyhád",
                                    Name = "G. Botond",
                                    NickNames = new List<string>{"Boti"}
                                },
                                new Player
                                {
                                    Institue = "Budapest",
                                    Name = "K. Fülöp",
                                    NickNames =new List<string> {}
                                },
                        },
                         TeamName = $"Csapat{i}",

                     });
            }

        }

        private ObservableCollection<Team> _listViewItemSource;
        public ObservableCollection<Team> ListViewItemSource
        {
            get => _listViewItemSource;
            set => SetProperty(ref _listViewItemSource, value);
        }

        private ICommand _removePlayerCommand;
        public ICommand RemovePlayerCommand => _removePlayerCommand ??= new RelayCommand<object>(On_RemovePlayer);

        public void On_RemovePlayer(object param)
        {
            var player = param as Player;
            PlayerTest.Remove(player);
        }

        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<Player>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var player in DummyData.DummyData.PLayers)
                {
                    var found = splitText.All((key) =>
                    {
                        return player.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(player);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add( new Player { Name = "Nem található játékos" });
                }
                sender.ItemsSource = suitableItems;
            }
        }

        public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var p = args.SelectedItem as Player;
            p.RemoveCommand = RemovePlayerCommand;
            PlayerTest.Add(p);
            sender.Text = string.Empty;
        }


        public ObservableCollection<Type> ReachablePages { get => new ObservableCollection<Type>(); set { } }

        public event EventHandler<NavigateToEventargs> NavigateTo;

        private ObservableCollection<Player> _playerTest;

        public ObservableCollection<Player> PlayerTest
        {
            get { return _playerTest; }
            set { SetProperty(ref _playerTest, value); }
        }

        public List<Team> TeamTest => DummyData.DummyData.Teams;

        private Visibility _gameSettingsVisible;
        public Visibility GameSettingsVisible
        {
            get => _gameSettingsVisible;
            set => SetProperty(ref _gameSettingsVisible, value);
        }

        private Visibility _roundSettingsVisible;
        public Visibility RoundSettingsVisible
        {
            get => _roundSettingsVisible;
            set => SetProperty(ref _roundSettingsVisible, value);
        }

        private ICommand _showRoundSettingsCommand;

        public ICommand ShowRoundSettingsCommand => _showRoundSettingsCommand ?? (_showRoundSettingsCommand = new RelayCommand(ShowRoundSettingsClicked));

        private void ShowRoundSettingsClicked()
        {
            GameSettingsVisible = Visibility.Collapsed;
            RoundSettingsVisible = Visibility.Visible;
        }

        private ICommand _showGameSettingsCommand;

        public ICommand ShowGameSettingsCommand => _showGameSettingsCommand ?? (_showGameSettingsCommand = new RelayCommand(ShowGameSettingsClicked));

        private void ShowGameSettingsClicked()
        {
            RoundSettingsVisible = Visibility.Collapsed;
            GameSettingsVisible = Visibility.Visible;
        }

        private ICommand _startGameCommand;

        public ICommand StartGameCommand => _startGameCommand ??= new RelayCommand(StartGameClicked);

        public int TeamSize 
        { 
            get => _teamSize; 
            set => SetProperty(ref _teamSize, value); 
        }

        private int _teamSize;


        private void BackgroundColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons rb)
            {
                string selectedValue;

                if(rb.SelectedItem is TextBox tb)
                {
                    //not validated
                    selectedValue = tb.Text;
                }
                else
                {
                    selectedValue = rb.SelectedItem as string;
                }

                TeamSize = int.Parse(selectedValue);              
            }
        }

        private ICommand _teamGameCheckBoxCommand;

        public ICommand TeamGameCheckBoxCommand => _teamGameCheckBoxCommand ??= new RelayCommand( () => TeamGame = !TeamGame);

        private bool _hasTematical;
        public bool HasTematical 
        { 
            get => _hasTematical; 
            set => SetProperty( ref _hasTematical, value); 
        }

        private bool _hasLightning;
        public bool HasLightning
        {
            get => _hasLightning;
            set => SetProperty(ref _hasLightning, value);
        }

        private ICommand _lightningCheckBoxCommand;

        public ICommand LightningCheckBoxCommand => _lightningCheckBoxCommand ??= new RelayCommand(() => HasLightning = !HasLightning);

        private ICommand _tematicalCheckBoxCommand;

        public ICommand TematicalCheckBoxCommand => _tematicalCheckBoxCommand ??= new RelayCommand(() => HasTematical = !HasTematical);

        public bool TeamGame 
        { 
            get => _teamGame; 
            set => SetProperty(ref _teamGame, value); 
        }
        

        private bool _teamGame;

        
        private void StartGameClicked()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(AllOrNothingTematicalViewModel), PageName = "Tematikus" });
        }

        public void ResetReachablePages()
        {
            throw new NotImplementedException();
        }
    }
}
