using AllOrNothing.Contracts.Services;
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
    public class AllOrNothingSettingsViewModel : ViewModelBase
    {

        public AllOrNothingSettingsViewModel(INavigationViewService navigationViewService, IMapper mapper, IUnitOfWork unitOfWork)
            : base(navigationViewService)
        {
            Mapper = mapper;
            _unitOfWork = unitOfWork;

            ResetSettings();
        }

        public void ResetSettings()
        {

            IsMenuButtonVisible = false;
            ReachablePages = null;

            _gameSettingsVisible = Visibility.Visible;
            _roundSettingsVisible = Visibility.Collapsed;
            _playerTest = new ObservableCollection<Player>();

            _gameModel = new GameModel(new GameSettingsModel(), new ObservableCollection<StandingDto>());
            _isRoundSettingsVisible = false;
            _rounds = null;
            _selectedRound = null;

            //_unitOfWork.QuestionSeries.Add(DummyData.DummyData.QS1);
            //_unitOfWork.Complete();

            _avaiblePlayers = new SortedSet<PlayerDto>(new PlayerDtoComparer());
            _avaiblePlayers.UnionWith(Mapper.Map<IEnumerable<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));

            var all = _unitOfWork.QuestionSeries.GetAllAvaible();
            var tmp = Mapper.Map<IEnumerable<QuestionSerie>, IEnumerable<QuestionSerieDto>>(all);

            foreach (var serie in tmp)
            {
                foreach (var topic in serie.Topics)
                {
                    topic.Questions.Sort(new QuestionDtoComparer());
                }
            }

            AvaibleSeries = new ObservableCollection<QuestionSerieDto>(tmp);

            _teams = new();
            _selectedPlayers = new();

            AllCompetences = _unitOfWork.Competences.GetAll().ToList();
        }

        private List<Competence> _allCompetences;
        public List<Competence> AllCompetences
        {
            get => _allCompetences;
            set => SetProperty(ref _allCompetences, value);
        }

        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private bool _canGoToNextPage;
        public bool CanGoToNextPage
        {
            get => true; //_canGoToNextPage;
            set => SetProperty(ref _canGoToNextPage, value);
        }

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
            if (await PopupManager.ShowDialog(PageXamlRoot, "Kilépés?", "Biztosan kilép?", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                ResetSettings();
                HidePage?.Invoke(this, "Beállítások");
                NavigateTo(this, new NavigateToEventargs { PageName = "Menu", PageVM = typeof(AllOrNothingViewModel) });
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

        public void TimeSelectionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (!checkBox.IsChecked.GetValueOrDefault(false))
                return;


            if (checkBox.Name == "tematicalCheckBox" && GameModel.GameSettings.GeneralTematicalTime.TotalSeconds == 0)
            {
                GameModel.GameSettings.GeneralTematicalTime = TimeSpan.FromHours(1);
            }
            else if (checkBox.Name == "lightningCheckBox" && GameModel.GameSettings.GeneralLightningTime.TotalSeconds == 0)
            {
                GameModel.GameSettings.GeneralLightningTime = TimeSpan.FromHours(1);
            }
            else if (checkBox.Name == "roundTematicalCheckBox" && SelectedRound.RoundSettings.TematicalTime.TotalSeconds == 0)
            {
                SelectedRound.RoundSettings.TematicalTime = TimeSpan.FromHours(1);
            }
            else if (checkBox.Name == "roundLightningCheckBox" && SelectedRound.RoundSettings.LightningTime.TotalSeconds == 0)
            {
                SelectedRound.RoundSettings.LightningTime = TimeSpan.FromHours(1);
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

        private async void LoadFromFileClicked()
        {

            AvaibleSeries = new ObservableCollection<QuestionSerieDto>(AvaibleSeries.Where(s => s.FromFile == false).ToList());

            var questionSerieLoader = Ioc.Default.GetService<QuestionSerieLoader>();
            string errorMessage = "";
            var series = questionSerieLoader.LoadAllSeriesFromFolder(App.QuestionSerieFolder, out errorMessage);

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
            var schedules = new List<Schedule>();

            var matrix = new int[teams.Count, teams.Count];
            var occurrences = new Dictionary<int, int>();

            for (int i = 0; i < teams.Count; i++)
            {
                occurrences.Add(i, 0);
            }
            bool isRunning = occurrences.Any(p => p.Value < numberOfRounds);
            //while there is a team, that didnt play the requiered times
            while (isRunning)
            {
                // the index of team in Teams
                var round = new List<int>();

                // we need a maximum of 4 team in a round
                for (int i = 0; i < 4 && isRunning; i++)
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

        private ICommand _removePlayerCommand;
        public ICommand RemovePlayerCommand => _removePlayerCommand ??= new RelayCommand<object>(On_RemovePlayer);

        private ICommand _navigateToNewPlayerPageCommand;
        public ICommand NavigateToNewPlayerPageCommand => _navigateToNewPlayerPageCommand ??= new RelayCommand(NavigateToNewPlayerPage);

        private void NavigateToNewPlayerPage()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageName = "Új játékos", PageVM = typeof(PlayerAddingViewModel) });
        }


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
            _avaiblePlayers.Add(playerDto);
        }

        public bool Asd => false;

        public void ItemDragStarting(UIElement sender, DragStartingEventArgs e)
        {
            e.AllowedOperations = DataPackageOperation.Move;
            var asd = e.Data;
        }

        private SortedSet<PlayerDto> _avaiblePlayers;

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
                        Text = "Nincs ilyen játkos!",
                    });

                    notfoundDisplay.Children.Add(new Button
                    {
                        Content = new TextBlock
                        {
                            Text = "Új játékos",
                        },
                        Command = NavigateToNewPlayerPageCommand,
                    });
                    suitableItems.Add(notfoundDisplay);
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
                NavigateTo?.Invoke(this, new NavigateToEventargs { PageName = "Új játékos", PageVM = typeof(PlayerAddingViewModel) });
            }

        }

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

        private bool HasGameValidationErrors(out string message)
        {
            message = "";
            var result = false;
            if (Teams.Count < 2)
            {
                result = true;
                message += "Legalább 2 csapatnak kell szerepelnie!\n";
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
                vm.InitVm(GameModel.GameStandings);

                GameSettingsVisible = Visibility.Collapsed;
                IsRoundSettingsVisible = true;
            }
        }

        private ICommand _startGameCommand;
        public ICommand StartGameCommand => _startGameCommand ??= new RelayCommand(StartGameClicked);

        private bool HasRoundValidationErrors(out string message)
        {
            message = "";          
            if (SelectedRound == null || SelectedRound.RoundEnded)
                message += "Válasszon ki egy le nem játszott kört!\n";

            if (SelectedRound != null && SelectedRound.RoundSettings.IsTematicalAllowed && SelectedRound.RoundSettings.QuestionSerie == null)
                message += "Válasszon ki egy kérdéssort!\n";
            else if(GameModel.GameSettings.IsGameWithoutButtonsEnabled && !SelectedRound.RoundSettings.QuestionSerie.CanBePlayedWithoutButtons)
            {
                message += "Olyan kérdéssort válasszon ki, ami kompatibilis a nyomógombok nélküli játékkal!\n";
            }

            if (SelectedRound != null && !(SelectedRound.RoundSettings.IsTematicalAllowed || SelectedRound.RoundSettings.IsLightningAllowed))
                message += "Legalább egy játékmódot válasszon ki!\n";

            return message != "";
        }

        private bool _gameInProgress;
        public bool GameInProgress
        {
            get => _gameInProgress;
            set => SetProperty(ref _gameInProgress, value);
        }
        private async void StartGameClicked()
        {
            var message = "";
            if (HasRoundValidationErrors(out message))
            {
                await PopupManager.ShowDialog(PageXamlRoot, "Hiba!", message, ContentDialogButton.Close, "", "Ok");
                return;
            }

            var vm = Ioc.Default.GetService<AllOrNothingGameViewModel>();          
            vm.SetupRound(SelectedRound);

            if (!GameInProgress)
            {
                vm.OccasionName = GameModel.GameSettings.OccasionName;
                vm.RoundOver += GameVM_RoundOver;
                GameInProgress = true;
            }
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(AllOrNothingGameViewModel), PageName = "Játék" }); 
        }

        private void GameVM_RoundOver(object sender, RoundModel e)
        {
            if (e.IsFinalRound)
            {
                IsMenuButtonVisible = false;
                GameInProgress = false;
            }
                

            //TODO: ténylege kell ez ide? nem elég csak az eredmények oldalában kezelni az eventet?
            var vm = Ioc.Default.GetService<ScoreBoardPageViewModel>();
            vm.UpdateStandings(e);
            if (e.IsFinalRound)
            {
                //TODO: az oldal eltűnést és megjelenést rendbrakni
                HidePage?.Invoke(this, "Beállítások");
            }
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(ScoreBoardPageViewModel), PageName = "Eredmények" });
        }

        public void ResetReachablePages()
        {
            throw new NotImplementedException();
        }


        private RoundModel _finalRound;
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
        private bool _isFinalRound;
        public bool IsFinalRound
        {
            get => _isFinalRound;
            set => SetProperty(ref _isFinalRound, value);
        }

        public IMapper Mapper
        {
            get => _mapper;
            set => SetProperty(ref _mapper, value);
        }



        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();

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
    }
}
