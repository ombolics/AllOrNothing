﻿using AllOrNothing.AutoMapper.Dto;
using AllOrNothing.Contracts.ViewModels;
using AllOrNothing.Controls;
using AllOrNothing.Data;
using AllOrNothing.Helpers;
using AllOrNothing.Models;
using AllOrNothing.Repository;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
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
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingSettingsViewModel : ObservableRecipient, INavigationAware
    {

        public AllOrNothingSettingsViewModel(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _gameSettingsVisible = Visibility.Visible;
            _roundSettingsVisible = Visibility.Collapsed;
            _listViewItemSource = new ObservableCollection<Team>();
            _playerTest = new ObservableCollection<Player>();

            _gameSettingsModel = new GameSettingsModel();
            _isRoundSettingsVisible = false;
            _selectedRound = null;

            _avaiblePlayers = new SortedSet<Player>(new PlayerComparer());
            _avaiblePlayers.UnionWith(_unitOfWork.Players.GetAll()); 
            _teams = new();
            _selectedPlayers = new();


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
                                    NickName = "Lajos",
                                },
                                new Player
                                {
                                    Institue = "Bonyhád",
                                    Name = "G. Botond",
                                    NickName = "Lajos",
                                },
                                new Player
                                {
                                    Institue = "Budapest",
                                    Name = "K. Fülöp",
                                    NickName = "Lajos",
                                },
                        },
                         TeamName = $"Csapat{i}",

                     });
            }


            Player p = new Player
            {
                Id = 0,
                Institue = "PSEG",
                Name = "Csabi",
                NickName = "Lajos",
            };

            var mapped = Ioc.Default.GetService<IMapper>().Map<PlayerDto>(p);

        }

        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private ICommand _loadFromFileCommand;
        public ICommand LoadFromFileCommand => _loadFromFileCommand ??= new RelayCommand(LoadFromFileClicked);

        public void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as AutoSuggestBox).ItemsSource = null;
        }

        public async void LoadFromFileClicked()
        {
            //FileOpenPicker picker = new FileOpenPicker();

            ////var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Current);

            //// Associate the HWND with the file picker
            //WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            //var file = await picker.PickSingleFileAsync();

        }
        public ObservableCollection<QuestionSerie> TestSeries => new ObservableCollection<QuestionSerie>(DummyData.DummyData.TestSeries);
        public void RoundSelected(object sender, ItemClickEventArgs e)
        {
            SelectedRound = e.ClickedItem as RoundSettingsModel;
        }

        private RoundSettingsModel _selectedRound;
        public RoundSettingsModel SelectedRound
        {
            get => _selectedRound;
            set => SetProperty(ref _selectedRound, value);
        }

        public ObservableCollection<RoundSettingsModel> Rounds
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
        private ObservableCollection<RoundSettingsModel> _rounds;


        private GameSettingsModel _gameSettingsModel;
        public GameSettingsModel GameSettingsModel 
        {
            get => _gameSettingsModel;
            set => SetProperty(ref _gameSettingsModel, value);
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
            while(helper.Count > 0)
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
                }
                value.Add(team);           
            }
            return value;
        }

       

        public void TeamsAllowedChecked(object sender , RoutedEventArgs e)
        {
            Teams = GenerateTeams(SelectedPlayers, GameSettingsModel.MaxTeamSize);
        }

        public void GenerateSchedule()
        {
            var Schedues = new List<Schedule>();
            var sch = new Schedule();
            //TODO generálási algoritmus
            for (int i = 0; i < Teams.Count; i++)
            {
                sch.Teams.Add(Teams[i]);
                if(i % 4 == 3 || i == Teams.Count-1)
                {
                    Schedues.Add(sch);
                    sch = new Schedule();
                }
            }

            GameSettingsModel.Schedules = Schedues;
            
        }

        public void On_PlayerDropped(object sender, Player player)
        {
            if(sender is TeamDto senderTeam)
            {
                var originalTeam = Teams
                    .Where(t => t.Players
                                    .Where(p => p.Id ==player.Id)
                                    .ToList().Count>0)
                    .ToList();

                foreach (var team in originalTeam)
                {
                    var removable = team.Players
                        .Where(p => p.Id == player.Id)
                        .FirstOrDefault();

                    team.Players.Remove(removable);
                }
                senderTeam.Players.Add(player);
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
                    suitableItems.Add( new Player { Name = "Nem található játékos" });
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
            GenerateSchedule();
            Rounds = new ObservableCollection<RoundSettingsModel>(RoundSettingsModel.FromGameSettingsModel(GameSettingsModel));
            SelectedRound = Rounds?[0];

            GameSettingsVisible = Visibility.Collapsed;
            IsRoundSettingsVisible = true;         
        }
   

        private ICommand _startGameCommand;
        public ICommand StartGameCommand => _startGameCommand ??= new RelayCommand(StartGameClicked);

        public int TeamSize 
        { 
            get => _teamSize; 
            set => SetProperty(ref _teamSize, value); 
        }

        private int _teamSize;

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
            var vm = Ioc.Default.GetService<AllOrNothingGameViewModel>();
            vm.SetupRound(SelectedRound);



            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(AllOrNothingGameViewModel), PageName = "Játék" });
            //TODO close this page
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
