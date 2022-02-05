using AllOrNothing.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
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

        public ObservableCollection<Type> ReachablePages { get => new ObservableCollection<Type>(); set { } }

        public event EventHandler<NavigateToEventargs> NavigateTo;


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
