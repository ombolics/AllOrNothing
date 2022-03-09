using CommunityToolkit.Mvvm.ComponentModel;
using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllOrNothing.DummyData;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using AllOrNothing.Models;
using System.Threading;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using AllOrNothing.Services;
using Windows.Storage.Pickers;
using System.Runtime.InteropServices;
using AllOrNothing.Contracts.ViewModels;
using AllOrNothing.Helpers;
using AllOrNothing.AutoMapper.Dto;
using AutoMapper;
using AllOrNothing.Controls;
using Windows.ApplicationModel.Core;
using CommunityToolkit.Mvvm.DependencyInjection;
using AllOrNothing.Views;

namespace AllOrNothing.ViewModels
{
    public enum GamePhase
    {
        TEMATICAL,
        LIGHTNING
    }

    public class AllOrNothingGameViewModel : ObservableRecipient, INavigationAware
    {
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
        public static extern IntPtr GetActiveWindow();

        public AllOrNothingGameViewModel( IMapper mapper)
        {
            ScoreTest = 3000;

            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += _gameTimer_Tick;
            _qsLoader = new QuestionSerieLoader();

            _currentStandings = new ObservableCollection<StandingDto>();
            _mapper = mapper;
        }
        private GamePhase _gamePhase;

        public void SetupRound(RoundSettingsModel m)
        {
            m.TematicalTime = m.TematicalTime.ShiftToRight();
            m.LightningTime = m.LightningTime.ShiftToRight();

            SelectedRound = m;

            if(SelectedRound.IsTematicalAllowed)
            {
                GamePhase = GamePhase.TEMATICAL;
            }
            else
            {
                GamePhase = GamePhase.LIGHTNING;
            }

            CurrentStandings = new ObservableCollection<StandingDto>();
            foreach (var team in m.Teams)
            {
                CurrentStandings.Add(new StandingDto
                {
                    Team = team,
                    Score = 0
                });
            }
        }

        private ObservableCollection<StandingDto> _currentStandings;
        public ObservableCollection<StandingDto> CurrentStandings
        {
            get => _currentStandings;
            set => SetProperty(ref _currentStandings, value);
        }


        private QuestionSerieLoader _qsLoader;
        

        private void _gameTimer_Tick(object sender, object e)
        {
            //var asd = SelectedRound.TematicalTime.ToString();
            
            if(GamePhase == GamePhase.TEMATICAL)
            {
                SelectedRound.TematicalTime = SelectedRound.TematicalTime.Subtract(TimeSpan.FromSeconds(1));
                if (SelectedRound.TematicalTime.TotalSeconds == 0)
                {
                    _gameTimer.Stop();
                }              
            }
            else
            {
                SelectedRound.LightningTime = SelectedRound.LightningTime.Subtract(TimeSpan.FromSeconds(1));
                if (SelectedRound.LightningTime.TotalSeconds == 0)
                {
                    _gameTimer.Stop();
                }               
            }
        }


        private RoundSettingsModel _selectedRound;


        private ICommand _toggleTimerCommand;
        public ICommand ToggleTimerCommand => _toggleTimerCommand ??= new RelayCommand(ToggleTimer);


        private ICommand _showLightningCommand;
        public ICommand ShowLightningCommand => _showLightningCommand ??= new RelayCommand(ShowLightning);

        private async void ShowLightning()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = PageXamlRoot;
            dialog.Title = "Villám?";
            dialog.PrimaryButtonText = "Villám!";
            dialog.CloseButtonText = "Mégse";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new LightningDialog();

            if(await dialog.ShowAsync(ContentDialogPlacement.Popup) == ContentDialogResult.Primary)
            {
                GamePhase = GamePhase.LIGHTNING;
                //CurrentQuestion
            }
        }

        private ICommand _loadFromFileCommand;
        public ICommand LoadFromFileCommand => _loadFromFileCommand ??= new RelayCommand(LoadFromFileClicked);

        public async void LoadFromFileClicked()
        {
            //FileOpenPicker picker = new FileOpenPicker();

            //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Current);

            //// Associate the HWND with the file picker
            //WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            //var file = await picker.PickSingleFileAsync();

        }

        private DispatcherTimer _gameTimer;

        private XamlRoot _pageXamlRoot;
        public XamlRoot PageXamlRoot
        {
            get => _pageXamlRoot;
            set => SetProperty(ref _pageXamlRoot, value);
        }

        private void ToggleTimer()
        {
            if ((GamePhase == GamePhase.TEMATICAL && SelectedRound.TematicalTime.TotalSeconds == 0) ||
                (GamePhase == GamePhase.LIGHTNING && SelectedRound.LightningTime.TotalSeconds == 0))
                return;

            if(_gameTimer.IsEnabled)
            {
                _gameTimer.Stop();
            }
            else
            {
                _gameTimer.Start();
            }
        }

        private int _scoreTest;
        public int ScoreTest 
        {
            get => _scoreTest;
            set => SetProperty(ref _scoreTest, value);
        }


        private readonly List<string> _enabledPages = new List<string> { "Beállítások", "Játék" };
        private readonly IMapper _mapper;

        /// <summary>
        /// <param>
        /// The members of the parameter are enabled
        /// </param>
        /// </summary>
        public event EventHandler<List<string>> HidePages;
        public void OnNavigatedTo(object parameter)
        {
            HidePages?.Invoke(this, _enabledPages);
        }

        public void OnNavigatedFrom()
        {
            
        }

        private QuestionDto _currentQuestion;
        public QuestionSerieDto Serie => _mapper.Map< QuestionSerieDto>( DummyData.DummyData.QS1);


        public QuestionDto CurrentQuestion 
        {
            get => _currentQuestion;
            set
            {
                SetProperty(ref _currentQuestion, value);
            } 
        }
        public RoundSettingsModel SelectedRound 
        { 
            get => _selectedRound; 
            set => SetProperty(ref _selectedRound, value); 
        }
        public GamePhase GamePhase 
        {
            get => _gamePhase; 
            set => SetProperty(ref _gamePhase, value); 
        }
    }
}
