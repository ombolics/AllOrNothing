using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Contracts.ViewModels;
using AllOrNothing.Controls;
using AllOrNothing.Data;
using AllOrNothing.Helpers;
using AllOrNothing.Models;
using AllOrNothing.Repository;
using AllOrNothing.Services;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingSettingsViewModel : ObservableRecipient, INavigationAware
    {

        public AllOrNothingSettingsViewModel(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            ResetSettings();
        }

        public void ResetSettings()
        {
            _gameSettingsVisible = Visibility.Visible;
            _roundSettingsVisible = Visibility.Collapsed;
            _playerTest = new ObservableCollection<Player>();

            _gameModel = new GameModel(new GameSettingsModel(), new ObservableCollection<StandingDto>());
            _isRoundSettingsVisible = false;
            _rounds = null;
            _selectedRound = null;

            //_unitOfWork.QuestionSeries.Add(DummyData.DummyData.QS1);
            //_unitOfWork.Complete();

            _avaiblePlayers = new SortedSet<Player>(new PlayerComparer());

            var all = _unitOfWork.QuestionSeries.GetAll();
            var tmp = _mapper.Map<IEnumerable<QuestionSerie>, IEnumerable<QuestionSerieDto>>(all);

            AvaibleSeries = new ObservableCollection<QuestionSerieDto>(tmp);
            _avaiblePlayers.UnionWith(_unitOfWork.Players.GetAll());
            _teams = new();
            _selectedPlayers = new();
        }

        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        

        private ICommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(Exit);

        private XamlRoot _pageXamlRoot;
        public XamlRoot PageXamlRoot
        {
            get => _pageXamlRoot;
            set => SetProperty(ref _pageXamlRoot, value);
        }
        public event EventHandler<string> HidePage;
        private async void Exit()
        {

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = PageXamlRoot;
            dialog.Title = "Kilépés?";
            dialog.PrimaryButtonText = "Igen";
            dialog.CloseButtonText = "Mégse";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new CustomDialog("Biztosan kilép?");

            if (await dialog.ShowAsync(ContentDialogPlacement.Popup) == ContentDialogResult.Primary)
            {
                ResetSettings();
                HidePage?.Invoke(this, "Beállítások");
                NavigateTo(this, new NavigateToEventargs { PageName = "Menu", PageVM = typeof(AllOrNothingViewModel) });
            }
            
        }

        private ICommand _generateTeamsCommand;
        public ICommand GenerateTeamsCommand => _generateTeamsCommand ??= new RelayCommand(GenerateTeamsClicked);

        private void GenerateTeamsClicked()
        {
            Teams = GenerateTeams(SelectedPlayers, GameModel.GameSettings.MaxTeamSize);
        }

        private ICommand _loadFromFileCommand;
        public ICommand LoadFromFileCommand => _loadFromFileCommand ??= new RelayCommand(LoadFromFileClicked);

        private void LoadFromFileClicked()
        {
            var questionSerieLoader = new QuestionSerieLoader();

            var series = questionSerieLoader.LoadAllSeriesFromFolder(App.QuestionSerieFolder);

            foreach (var serie in series)
            {
                var dto = _mapper.Map<QuestionSerieDto>(serie);
                dto.FromFile = true;
                if(dto.Competences.Count == 0)
                {
                    dto.Topics[0].Competences = new List<CompetenceDto>{new CompetenceDto
                    {
                        Name = "Nem ismert",
                    }};
                }

                if (dto.Authors.Count == 0)
                {
                    dto.Topics[0].Author = new PlayerDto
                    {
                        Name = "Nem ismert",
                    };
                }

                AvaibleSeries.Add(dto);
            }

        }

        public void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as AutoSuggestBox).ItemsSource = null;
        }

        private ObservableCollection<QuestionSerieDto> _avaibleSeries;
        public ObservableCollection<QuestionSerieDto> AvaibleSeries
        {
            get => _avaibleSeries;
            set => SetProperty(ref _avaibleSeries, value);
        }
        public void RoundSelected(object sender, ItemClickEventArgs e)
        {
            SelectedRound = e.ClickedItem as RoundModel;
        }

        private RoundModel _selectedRound;
        public RoundModel SelectedRound
        {
            get => _selectedRound;
            set => SetProperty(ref _selectedRound, value);
        }

        public ObservableCollection<RoundModel> Rounds
        {
            get => _rounds;
            set
            {
                SetProperty(ref _rounds, value);
                //SelectedRound = _rounds[0];
            }
        }

        private bool _isRoundSettingsVisible;
        public bool IsRoundSettingsVisible
        {
            get => _isRoundSettingsVisible;
            set => SetProperty(ref _isRoundSettingsVisible, value);
        }
        private ObservableCollection<RoundModel> _rounds;


        private GameModel _gameModel;
        public GameModel GameModel
        {
            get => _gameModel;
            set => SetProperty(ref _gameModel, value);
        }

        //private ICommand _dropPlayerCommand;
        //public ICommand DropPlayerCommand => _dropPlayerCommand ??= new RelayCommand<PlayerDto>(OnPlayerDropped);

        public void FinalizeSettings()
        {
            var roundSettings = new List<RoundSettingsModel>();
            //TODO értelmesen megcsinálni
            var tmp = new RoundSettingsModel();

        }

        private ObservableCollection<TeamDto> GenerateTeams(ICollection<PlayerDto> players, int maxTeamSize)
        {
            var value = new ObservableCollection<TeamDto>();

            List<Player> helper = _mapper.Map<ICollection<PlayerDto>, List<Player>>(players);
            Random r = new Random();
            while (helper.Count > 0)
            {
                var team = new TeamDto
                {
                    Players = new(),
                };
                for (int i = 0; i < maxTeamSize && helper.Count > 0; i++)
                {
                    var plyr = helper[r.Next(0, helper.Count)];

                    //plyr.OriginalTeam = team.Players;

                    helper.Remove(plyr);
                    team.Players.Add(plyr);
                    team.TeamName += plyr.NickName + " - ";

                    team.PlayerDrop += On_PlayerDropped;

                    team.UpdateTeamName();
                }
                value.Add(team);
            }
            return value;
        }



        //public void TeamsAllowedChecked(object sender, RoutedEventArgs e)
        //{
        //    Teams = GenerateTeams(SelectedPlayers, GameModel.GameSettings.MaxTeamSize);
        //}

        private int RoundsAgainstEachOther(int ind1, int ind2, int[,] matrix)
        {
            if (matrix[ind1, ind2] != matrix[ind2, ind1])
                throw new ArgumentException("The matrix must be symmetrical!");
            return matrix[ind1, ind2];
        }

        private int SumOfPlayedAgainstTeamsInRound(int teamIndex, List<int> round, int[,] matrix)
        {
            int value = 0;
            int i = 0;

            while (i < round.Count && round[i] > 0)
            {
                value += RoundsAgainstEachOther(teamIndex, round[i], matrix);
                i++;
            }
            return value;
        }

        private void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Debug.Write("[");
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Debug.Write(matrix[i, j] + "|");
                }
                Debug.WriteLine("]");
            }
        }

        public void GenerateSchedule()
        {
            var Schedules = new List<Schedule>();

            var matrix = new int[Teams.Count, Teams.Count];
            var occurrences = new Dictionary<int, int>();

            for (int i = 0; i < Teams.Count; i++)
            {
                occurrences.Add(i, 0);
            }

            while (occurrences.Any(p => p.Value < GameModel.GameSettings.NumberOfRounds))
            {

                var round = new List<int>();

                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        int teamIndex = occurrences.Where(p => p.Value == occurrences.Min(p => p.Value)).ElementAt(0).Key;
                        round.Add(teamIndex);
                        occurrences[teamIndex]++;
                    }
                    else
                    {
                        var minTeams = occurrences.Where(p => p.Value == occurrences.Min(p => p.Value)).ToList();

                        int min = int.MaxValue;
                        int minIndex = -1;
                        for (int j = 0; j < minTeams.Count; j++)
                        {
                            var val = SumOfPlayedAgainstTeamsInRound(minTeams[j].Key, round, matrix);
                            if (val < min)
                            {
                                min = val;
                                minIndex = minTeams[j].Key;
                            }
                        }

                        round.Add(minIndex);
                        occurrences[minIndex]++;
                        foreach (var item in round)
                        {
                            matrix[item, minIndex]++;
                            matrix[minIndex, item]++;
                        }
                    }
                }

                var sch = new Schedule();
                foreach (var item in round)
                {
                    sch.Teams.Add(Teams[item]);
                }

                Schedules.Add(sch);
            }
            PrintMatrix(matrix);
            GameModel.GameSettings.Schedules = Schedules;
        }

        public IList<StandingDto> CreateStandingFromGameModel(GameModel model)
        {
            var val = new List<StandingDto>();
            foreach (var item in Teams)
            {
                val.Add(new StandingDto
                {
                    Team = item,
                    MatchPlayed = 0,
                    Score = 0,
                }) ;
            }
            return val;
        }

        public void On_PlayerDropped(object sender, Player player)
        {
            if (sender is TeamDto senderTeam)
            {
                var originalTeam = Teams
                    .Where(t => t.Players
                                    .Where(p => p.Id == player.Id)
                                    .ToList().Count > 0)
                    .ToList();

                foreach (var team in originalTeam)
                {
                    var removable = team.Players
                        .Where(p => p.Id == player.Id)
                        .FirstOrDefault();

                    team.Players.Remove(removable);
                    team.UpdateTeamName();
                }
                senderTeam.Players.Add(player);
                senderTeam.UpdateTeamName();


            }
        }

        private ICommand _removePlayerCommand;
        public ICommand RemovePlayerCommand => _removePlayerCommand ??= new RelayCommand<object>(On_RemovePlayer);

        private ObservableCollection<TeamDto> _teams;
        public ObservableCollection<TeamDto> Teams
        {
            get => _teams;
            set => SetProperty(ref _teams, value);
        }

        public void On_RemovePlayer(object param)
        {
            var playerDto = param as PlayerDto;
            SelectedPlayers.Remove(playerDto);

            var player = _mapper.Map<Player>(playerDto);
            _avaiblePlayers.Add(player);
        }

        public void ItemDragStarting(UIElement sender, DragStartingEventArgs e)
        {
            e.AllowedOperations = DataPackageOperation.Move;
            var asd = e.Data;
        }

        private SortedSet<Player> _avaiblePlayers;

        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<Player>();
                var splitText = sender.Text.ToLower().Split(" ");

                //TODO Search for players in database

                foreach (var player in _avaiblePlayers)
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
                    suitableItems.Add(new Player { Name = "Nem található játékos" });
                }

                sender.ItemsSource = suitableItems;

            }
        }

        private ObservableCollection<PlayerDto> _selectedPlayers;
        public ObservableCollection<PlayerDto> SelectedPlayers
        {
            get => _selectedPlayers;
            set => SetProperty(ref _selectedPlayers, value);
        }


        public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

            //TODO Convert players to playerDTo
            var p = args.SelectedItem as Player;
            _avaiblePlayers.Remove(p);

            var dto = _mapper.Map<PlayerDto>(p);

            dto.RemoveCommand = RemovePlayerCommand as RelayCommand<object>;
            SelectedPlayers.Add(dto);
            sender.Text = string.Empty;
            sender.ItemsSource = null;
        }


        public ObservableCollection<Type> ReachablePages { get => new ObservableCollection<Type>(); set { } }

        public event EventHandler<NavigateToEventargs> NavigateTo;

        private ObservableCollection<Player> _playerTest;

        public ObservableCollection<Player> PlayerTest
        {
            get { return _playerTest; }
            set { SetProperty(ref _playerTest, value); }
        }

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
            //TODO validáció
            GenerateSchedule();
            GameModel.GameStandings = new ObservableCollection<StandingDto>(CreateStandingFromGameModel(GameModel));

            Rounds = new ObservableCollection<RoundModel>(RoundModel.FromGameModel(GameModel));
            SelectedRound = Rounds?[0];

            var vm = Ioc.Default.GetService<ScoreBoardPageViewModel>();

            vm.GameStandings = new List<StandingDto>(GameModel.GameStandings);

            GameSettingsVisible = Visibility.Collapsed;
            IsRoundSettingsVisible = true;
        }

        private ICommand _startGameCommand;
        public ICommand StartGameCommand => _startGameCommand ??= new RelayCommand(StartGameClicked);

        private void StartGameClicked()
        {
            var vm = Ioc.Default.GetService<AllOrNothingGameViewModel>();
            vm.SetupRound(SelectedRound);
            vm.RoundOver += GameVM_RoundOver;

            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(AllOrNothingGameViewModel), PageName = "Játék" });
            //TODO close this page
        }

        private void GameVM_RoundOver(object sender, RoundModel e)
        {
            var vm = Ioc.Default.GetService<ScoreBoardPageViewModel>();
            vm.Setup(e);

            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(ScoreBoardPageViewModel), PageName = "Eredmények" });
        }

        public void ResetReachablePages()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<List<string>> HidePages;

        public void OnNavigatedTo(object parameter)
        {
            HidePages?.Invoke(this, null);
        }

        public void OnNavigatedFrom()
        {

        }
    }
}
