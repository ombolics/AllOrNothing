using AllOrNothing.Contracts.Services;
using AllOrNothing.Contracts.ViewModels;
using AllOrNothing.Controls;
using AllOrNothing.Data;
using AllOrNothing.Helpers;
using AllOrNothing.Mapping;
using AllOrNothing.Models;
using AllOrNothing.Repository;
using AllOrNothing.Services;
using AutoMapper;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;

namespace AllOrNothing.ViewModels
{
    public class GameSettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuestionSerieLoader _questionSerieLoader;

        private List<Competence> _allCompetences;
        private bool _canGoToNextPage;
        private XamlRoot _pageXamlRoot;
        private ObservableCollection<QuestionSerieDto> _avaibleSeries;
        private RoundModel _selectedRound;
        private ObservableCollection<RoundModel> _rounds;
        private GameModel _gameModel;
        private ObservableCollection<PlayerDto> _selectedPlayers;
        private SortedSet<PlayerDto> _avaiblePlayers;
        private bool _isGameSettingsVisible;
        private bool _gameInProgress;
        private RoundModel _finalRound;
        private bool _isFinalRound;
        private ObservableCollection<TeamDto> _teams;
        private bool _pageEnabled;

        private ICommand _exitCommand;
        private ICommand _generateTeamsCommand;
        private ICommand _loadFromFileCommand;
        private ICommand _removePlayerCommand;
        private ICommand _showRoundSettingsCommand;
        private ICommand _startGameCommand;
        private ICommand _navigateToNewPlayerPageCommand;
        #endregion

        #region Constructors
        public GameSettingsViewModel(INavigationViewService navigationViewService, IMapper mapper, IUnitOfWork unitOfWork, IQuestionSerieLoader questionSerieLoader)
            : base(navigationViewService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _questionSerieLoader = questionSerieLoader;

            ResetSettings();
        }
        #endregion

        #region Properties
        public IMapper Mapper => _mapper;
        public List<Competence> AllCompetences
        {
            get => _allCompetences;
            set => SetProperty(ref _allCompetences, value);
        }
        public bool CanGoToNextPage
        {
            get => true; //_canGoToNextPage;
            set => SetProperty(ref _canGoToNextPage, value);
        }
        public XamlRoot PageXamlRoot
        {
            get => _pageXamlRoot;
            set => SetProperty(ref _pageXamlRoot, value);
        }
        public ObservableCollection<QuestionSerieDto> AvaibleSeries
        {
            get => _avaibleSeries;
            set => SetProperty(ref _avaibleSeries, value);
        }
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
        public GameModel GameModel
        {
            get => _gameModel;
            set => SetProperty(ref _gameModel, value);
        }
        public ObservableCollection<PlayerDto> SelectedPlayers
        {
            get => _selectedPlayers;
            set => SetProperty(ref _selectedPlayers, value);
        }
        public RoundModel FinalRound
        {
            get => _finalRound;
            set
            {
                SetProperty(ref _finalRound, value);
                if (value != null)
                    IsFinalRound = true;
            }
        }
        public ObservableCollection<TeamDto> Teams
        {
            get => _teams;
            set => SetProperty(ref _teams, value);
        }
        public bool IsFinalRound
        {
            get => _isFinalRound;
            set => SetProperty(ref _isFinalRound, value);
        }
        public bool IsGameSettingsVisible
        {
            get => _isGameSettingsVisible;
            set => SetProperty(ref _isGameSettingsVisible, value);
        }
        public bool GameInProgress
        {
            get => _gameInProgress;
            set => SetProperty(ref _gameInProgress, value);
        }
        public bool PageEnabled
        {
            get => _pageEnabled;
            set => SetProperty(ref _pageEnabled, value);
        }

        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(Exit);
        public ICommand GenerateTeamsCommand => _generateTeamsCommand ??= new RelayCommand(GenerateTeamsClicked);
        public ICommand LoadFromFileCommand => _loadFromFileCommand ??= new RelayCommand(LoadFromFileClicked);
        public ICommand ShowRoundSettingsCommand => _showRoundSettingsCommand ?? (_showRoundSettingsCommand = new RelayCommand(ShowRoundSettingsClicked));
        public ICommand StartGameCommand => _startGameCommand ??= new RelayCommand(StartGameClicked);
        public ICommand RemovePlayerCommand => _removePlayerCommand ??= new RelayCommand<object>(On_RemovePlayer);
        public ICommand NavigateToNewPlayerPageCommand => _navigateToNewPlayerPageCommand ??= new RelayCommand(NavigateToNewPlayerPage);
        #endregion

        #region Methods
        public void ResetSettings()
        {
            IsMenuButtonVisible = false;
            ReachablePages = null;
            IsGameSettingsVisible = true;
            _gameModel = new GameModel(new GameSettingsModel(), new ObservableCollection<StandingDto>());
            _rounds = null;
            _selectedRound = null;
            _avaiblePlayers = new SortedSet<PlayerDto>(new PlayerDtoComparer());
            _avaiblePlayers.UnionWith(Mapper.Map<IEnumerable<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
            _teams = new();
            _selectedPlayers = new();
            FinalRound = null;
            AllCompetences = _unitOfWork.Competences.GetAll().ToList();

            var series = Mapper.Map<IEnumerable<QuestionSerie>, IEnumerable<QuestionSerieDto>>(_unitOfWork.QuestionSeries.GetAllAvaible());
            foreach (var serie in series)
            {
                foreach (var topic in serie.Topics)
                {
                    topic.Questions.Sort(new QuestionDtoComparer());
                }
            }
            AvaibleSeries = new ObservableCollection<QuestionSerieDto>(series);  
        }

        private async void Exit()
        {
            if (await PopupManager.ShowDialog(PageXamlRoot, "Kilépés?", "Biztosan kilép?", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                ResetSettings();
                IsMenuButtonVisible = false;
                RaiseNavigateTo(new NavigateToEventArgs { PageName = "Főmenü", PageVM = typeof(MainMenuViewModel) });
            }
        }

        public void TimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs e)
        {
            if (e.NewTime.Value.TotalSeconds != 0)
                return;

            if (sender.Name == "tematicalTimePicker")
            {
                GameModel.GameSettings.IsTematicalAllowed = false;
            }
            else if (sender.Name == "lightningTimePicker")
            {
                GameModel.GameSettings.IsLightningAllowed = false;
            }
            else if (sender.Name == "roundLightningTime")
            {
                SelectedRound.RoundSettings.IsLightningAllowed = false;
            }
            else if (sender.Name == "roundTematicalTime")
            {
                SelectedRound.RoundSettings.IsTematicalAllowed = false;
            }
        }

        public void GmaWithoutButtonsChecked(object sender, RoutedEventArgs e)
        {
            GameModel.GameSettings.IsLightningAllowed = false;
        }

        public void LightningCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (!checkBox.IsChecked.GetValueOrDefault(false))
                return;

            if(checkBox.Name == "lightningCheckBox")
            {
                GameModel.GameSettings.IsGameWithoutButtonsEnabled = false;
                if (GameModel.GameSettings.GeneralLightningTime.TotalSeconds == 0)
                {
                    GameModel.GameSettings.GeneralLightningTime = TimeSpan.FromHours(1);
                }
                return;
            }

            if (checkBox.Name == "roundLightningCheckBox")
            {
                SelectedRound.RoundSettings.LightningTime = TimeSpan.FromHours(1);
                return;
            }
            
        }

        public void TematicalCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (!checkBox.IsChecked.GetValueOrDefault(false))
                return;

            if (checkBox.Name == "tematicalCheckBox" && GameModel.GameSettings.GeneralTematicalTime.TotalSeconds == 0)
            {
                GameModel.GameSettings.GeneralTematicalTime = TimeSpan.FromHours(1);
            }         
            else if (checkBox.Name == "roundTematicalCheckBox" && SelectedRound.RoundSettings.TematicalTime.TotalSeconds == 0)
            {
                SelectedRound.RoundSettings.TematicalTime = TimeSpan.FromHours(1);
            }          
        }

        private void GenerateTeamsClicked()
        {
            Teams = GenerateTeams(SelectedPlayers, GameModel.GameSettings.MaxTeamSize);
        }

        private async void LoadFromFileClicked()
        {

            AvaibleSeries = new ObservableCollection<QuestionSerieDto>(AvaibleSeries.Where(s => s.FromFile == false).ToList());

            string errorMessage = "";
            var series = _questionSerieLoader.LoadAllSeriesFromFolder(App.QuestionSerieFolder, out errorMessage);

            foreach (var serie in series)
            {
                var dto = Mapper.Map<QuestionSerieDto>(serie);
                dto.FromFile = true;
                if (dto.Competences == null || dto.Competences.Count == 0)
                {
                    dto.Topics[0].Competences = new ObservableCollection<CompetenceDto>{new CompetenceDto
                    {
                        Name = "Nem ismert",
                    }};
                }

                if (dto.Authors == null || dto.Authors.Count == 0)
                {
                    dto.Topics[0].Author = new PlayerDto
                    {
                        Name = "Nem ismert",
                    };
                }
                AvaibleSeries.Add(dto);
            }

            if (errorMessage != "")
            {
                await PopupManager.ShowDialog(PageXamlRoot, "Hiba a betöltéskor!", errorMessage, ContentDialogButton.Close, "", "Ok");
            }
        }

        public void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as AutoSuggestBox).ItemsSource = null;
        }
 
        public void RoundSelected(object sender, ItemClickEventArgs e)
        {
            SelectedRound = e.ClickedItem as RoundModel;
        }

        public ObservableCollection<TeamDto> GenerateTeams(ICollection<PlayerDto> players, int maxTeamSize)
        {
            var value = new ObservableCollection<TeamDto>();

            List<DragablePlayer> helper = Mapper.Map<ICollection<PlayerDto>, List<DragablePlayer>>(players);
            Random r = new Random();
            while (helper.Count > 0)
            {
                var team = new TeamDto
                {
                    Players = new(),
                    MaxPlayerCount = GameModel.GameSettings.MaxTeamSize,
                    MinPlayerCount = 1,
                };
                team.Players.CollectionChanged += team.CheckGuards;

                for (int i = 0; i < maxTeamSize && helper.Count > 0; i++)
                {
                    var plyr = helper[r.Next(0, helper.Count)];

                    //plyr.OriginalTeam = team.Players;

                    plyr.SwitchPlayers += Plyr_SwitchPlayers;
                    helper.Remove(plyr);
                    team.Players.Add(plyr);
                    team.TeamName += plyr.NickName + " - ";
                    team.PlayerDrop += On_PlayerDropped;

                    team.UpdateTeamName();
                }

                team.CheckGuards(this, null);
                value.Add(team);
            }
            return value;
        }

        private void Plyr_SwitchPlayers(object sender, DragablePlayer e)
        {
            var player1 = sender as DragablePlayer;
            DragablePlayer player2 = null;
            for (int i = 0; i < Teams.Count && player2 == null; i++)
            {
                player2 = Teams[i].TryGetPlayerById(e.Id);
            }
            var team1 = Teams.Single(t => t.Players.Any(p => p.Id == player1.Id));
            var team2 = Teams.Single(t => t.Players.Any(p => p.Id == player2.Id));

            team1.Players.Remove(player1);
            team1.Players.Add(player2);
            team2.Players.Remove(player2);
            team2.Players.Add(player1);

            team1.UpdateTeamName();
            team2.UpdateTeamName();
        }

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
            value = round.Sum(r => RoundsAgainstEachOther(teamIndex, r, matrix));

            //while (i < round.Count && round[i] > 0)
            //{
            //    value += RoundsAgainstEachOther(teamIndex, round[i], matrix);
            //    i++;
            //}
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

        public List<Schedule> GenerateSchedule(IList<TeamDto> teams, int numberOfRounds)
        {
            if (teams.Count < 4)
                return null;
            int totalAppereaceCount = teams.Count * numberOfRounds;
            List<int> roundCounts = new List<int>();

            switch (totalAppereaceCount % 4)
            {
                case 0:
                    for (int i = 0; i < totalAppereaceCount/4; i++)
                    {
                        roundCounts.Add(4);
                    }
                    break;
                case 1:
                    if(totalAppereaceCount > 5)
                    {
                        for (int i = 0; i < (totalAppereaceCount - 9) / 4; i++)
                        {
                            roundCounts.Add(4);
                        }
                        roundCounts.Add(3);
                        roundCounts.Add(3);
                        roundCounts.Add(3);
                    }
                    else
                    {
                        roundCounts.Add(3);
                        roundCounts.Add(2);
                    }
                    break;
                case 2:
                    if (totalAppereaceCount > 6)
                    {
                        for (int i = 0; i < (totalAppereaceCount - 6) / 4; i++)
                        {
                            roundCounts.Add(4);
                        }
                        roundCounts.Add(3);
                        roundCounts.Add(3);
                    }
                    else
                    {
                        roundCounts.Add(3);
                        roundCounts.Add(3);
                    }
                    break;
                case 3:
                    for (int i = 0; i < (totalAppereaceCount - 3) / 4; i++)
                    {
                        roundCounts.Add(4);
                    }
                    roundCounts.Add(3);
                    break;
                default:
                    break;
            }

            var schedules = new List<Schedule>();
            var matrix = new int[teams.Count, teams.Count];
            var occurrences = new Dictionary<int, int>();

            for (int i = 0; i < teams.Count; i++)
            {
                occurrences.Add(i, 0);
            }
            bool isRunning = occurrences.Any(p => p.Value < numberOfRounds);



            foreach (var roundCount in roundCounts)
            {
                // the index of team in Teams
                var round = new List<int>();
                // we need a maximum of 4 team in a round
                for (int i = 0; i < roundCount; i++)
                {
                    if (i == 0)
                    {
                        int teamIndex = occurrences.First(p => p.Value == occurrences.Min(p => p.Value)).Key;
                        round.Add(teamIndex);
                        occurrences[teamIndex]++;
                    }
                    else
                    {
                        //teams with the least occurences
                        var minTeams = occurrences.Where(p => p.Value == occurrences.Min(p => p.Value)).ToList();

                        int min = int.MaxValue;
                        int minIndex = -1;
                        for (int j = 0; j < minTeams.Count; j++)
                        {
                            var val = SumOfPlayedAgainstTeamsInRound(minTeams[j].Key, round, matrix);
                            if (val < min && !round.Contains(minTeams[j].Key))
                            {
                                min = val;
                                minIndex = minTeams[j].Key;
                            }
                        }

                        round.Add(minIndex);
                        occurrences[minIndex]++;
                        foreach (var item in round)
                        {
                            if (item == minIndex)
                                continue;
                            matrix[item, minIndex]++;
                            matrix[minIndex, item]++;
                        }
                    }
                    isRunning = occurrences.Any(p => p.Value < numberOfRounds);
                }

                var sch = new Schedule();
                foreach (var item in round)
                {
                    sch.Teams.Add(teams[item]);
                }

                schedules.Add(sch);
            }
            PrintMatrix(matrix);
            return schedules;
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
                });
            }
            return val;
        }

        public void On_PlayerDropped(object sender, DragablePlayer player)
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

        private void NavigateToNewPlayerPage()
        {
            RaiseNavigateTo(new NavigateToEventArgs { PageName = "Játékosok", PageVM = typeof(PlayerAddingViewModel) });
        }

        public void On_RemovePlayer(object param)
        {
            var playerDto = param as PlayerDto;
            SelectedPlayers.Remove(playerDto);
            _avaiblePlayers.Add(playerDto);
        }

        public void ItemDragStarting(UIElement sender, DragStartingEventArgs e)
        {
            e.AllowedOperations = DataPackageOperation.Move;
        }

        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<object>();
                var splitText = sender.Text.ToLower().Split(" ");

                foreach (var player in _avaiblePlayers)
                {
                    //found
                    if (splitText.All((key) => player.Name.ToLower().Contains(key)))
                    {
                        suitableItems.Add(Mapper.Map<PlayerDto>(player));
                    }
                }

                if (suitableItems.Count == 0)
                {
                    var notfoundDisplay = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 30.0,
                    };

                    notfoundDisplay.Children.Add(new TextBlock
                    {
                        Text = $"Kattintson a(z) {sender.Text} nevű játékos hozzáadásához!",
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.WrapWholeWords,
                    });
                    
                    suitableItems.Add(notfoundDisplay);
                }
                sender.ItemsSource = suitableItems;
            }
        }

        public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is PlayerDto player)
            {
                _avaiblePlayers.Remove(player);
                player.RemoveCommand = RemovePlayerCommand as RelayCommand<object>;
                SelectedPlayers.Add(player);
                sender.Text = string.Empty;
                sender.ItemsSource = null;
                return;
            }

            if (args.SelectedItem is StackPanel notFoundDisplay)
            {
                RaiseNavigateTo(new NavigateToEventArgs { PageName = "Játékosok", PageVM = typeof(PlayerAddingViewModel) });
            }

        }

        private bool HasGameValidationErrors(out string message)
        {
            message = "";
            var result = false;
            if (Teams.Count < 4)
            {
                result = true;
                message += "Legalább 4 csapatnak kell szerepelnie!\n";
            }

            if (!(GameModel.GameSettings.IsLightningAllowed || GameModel.GameSettings.IsTematicalAllowed))
            {
                result = true;
                message += "Legalább egy játékmódot válasszon ki!\n";
            }
            return result;
        }

        private async void ShowRoundSettingsClicked()
        {
            var message = "";
            if (HasGameValidationErrors(out message))
            {
                await PopupManager.ShowDialog(PageXamlRoot, "Hiba!", message, ContentDialogButton.Close, "", "Ok");
            }
            else
            {
                GameModel.GameSettings.Schedules = GenerateSchedule(Teams, GameModel.GameSettings.NumberOfRounds);
                GameModel.GameStandings = new ObservableCollection<StandingDto>(CreateStandingFromGameModel(GameModel));

                Rounds = new ObservableCollection<RoundModel>(RoundModel.FromGameModel(GameModel));
                SelectedRound = Rounds?[0];

                var vm = Ioc.Default.GetService<ScoreBoardPageViewModel>();
                vm.Init(GameModel.GameStandings);

                IsGameSettingsVisible = false;
            }
        }

        private bool HasRoundValidationErrors(out string message)
        {
            message = "";          
            if (SelectedRound == null || SelectedRound.RoundEnded)
                message += "Válasszon ki egy le nem játszott kört!\n";

            if (SelectedRound != null && SelectedRound.RoundSettings.IsTematicalAllowed && SelectedRound.RoundSettings.QuestionSerie == null)
                message += "Válasszon ki egy kérdéssort!\n";
            else if(GameModel.GameSettings.IsGameWithoutButtonsEnabled && !SelectedRound.RoundSettings.QuestionSerie.CanBePlayedWithoutButtons)
            {
                message += "Olyan kérdéssort válasszon ki, ami kompatibilis az automatizált játékkal!\n";
            }

            if (SelectedRound != null && !(SelectedRound.RoundSettings.IsTematicalAllowed || SelectedRound.RoundSettings.IsLightningAllowed))
                message += "Legalább egy játékmódot válasszon ki!\n";

            return message != "";
        }
        
        private async void StartGameClicked()
        {
            var message = "";
            if (HasRoundValidationErrors(out message))
            {
                await PopupManager.ShowDialog(PageXamlRoot, "Hiba!", message, ContentDialogButton.Close, "", "Ok");
                return;
            }

            var vm = Ioc.Default.GetService<GameViewModel>();    
            vm.SetupRound(SelectedRound);
            SelectedRound.RoundStarted = true;

            if (!GameInProgress)
            {
                vm.OccasionName = GameModel.GameSettings.OccasionName;
                vm.RoundOver += GameVM_RoundOver;
                GameInProgress = true;
            }
            RaiseNavigateTo(new NavigateToEventArgs { PageVM = typeof(GameViewModel), PageName = "Játék" });
        }

        private void GameVM_RoundOver(object sender, RoundModel e)
        {
            if (e.IsFinalRound)
            {
                IsMenuButtonVisible = false;
                GameInProgress = false;
            }              

            var vm = Ioc.Default.GetService<ScoreBoardPageViewModel>();
            vm.UpdateStandings(e);
            if (e.IsFinalRound)
            {
                IsMenuButtonVisible = false;
            }
            RaiseNavigateTo(new NavigateToEventArgs { PageVM = typeof(ScoreBoardPageViewModel), PageName = "Eredmények" });
        }
        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            _avaiblePlayers.UnionWith(Mapper.Map<IEnumerable<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
            _avaiblePlayers.RemoveWhere(p => SelectedPlayers.Any(p1 => p1.Id == p.Id));

            PageEnabled = Rounds == null ? true : !Rounds.Any(rm => rm.RoundStarted && !rm.RoundEnded);

            if (FinalRound == null && Rounds != null && Rounds.All(r => r.RoundEnded))
            {
                var teams = GameModel.GameStandings
                    .OrderByDescending(s => s.Score)
                    .Take(4)
                    .Select(s => s.Team)
                    .ToList();
                FinalRound = new RoundModel(teams, GameModel.GameSettings);
                FinalRound.IsFinalRound = true;
                SelectedRound = FinalRound;
            }
        }
        #endregion
    }
}
