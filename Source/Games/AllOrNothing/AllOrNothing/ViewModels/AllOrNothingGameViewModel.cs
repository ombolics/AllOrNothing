using AllOrNothing.Contracts.Services;
using AllOrNothing.Controls;
using AllOrNothing.Helpers;
using AllOrNothing.Mapping;
using AllOrNothing.Models;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Storage.Pickers;

namespace AllOrNothing.ViewModels
{
    public enum GamePhase
    {
        TEMATICAL,
        LIGHTNING
    }

    public class AllOrNothingGameViewModel : ViewModelBase
    {
        public AllOrNothingGameViewModel(INavigationViewService navigationViewService, IMapper mapper) : base(navigationViewService)
        {
            ScoreTest = 3000;

            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += _gameTimer_Tick;
            _mapper = mapper;

            ReachablePages = new List<Type> { typeof(AllOrNothingSettingsViewModel) };
        }
        private GamePhase _gamePhase;

        private ObservableCollection<AnswerLogModel> _answerLog;
        public ObservableCollection<AnswerLogModel> AnswerLog
        {
            get => _answerLog;
            set => SetProperty(ref _answerLog, value);
        }


        public event EventHandler<string> HidePage;

        public void SetupRound(RoundModel m)
        {
            m.RoundSettings.TematicalTime = m.RoundSettings.TematicalTime.ShiftToRight();
            m.RoundSettings.LightningTime = m.RoundSettings.LightningTime.ShiftToRight();

            SelectedRound = m;

            if (SelectedRound.RoundSettings.IsTematicalAllowed)
            {
                GamePhase = GamePhase.TEMATICAL;
            }
            else
            {
                GamePhase = GamePhase.LIGHTNING;
            }


            if (SelectedRound.RoundSettings.IsGameWithoutButtonsEnabled)
            {
                AnswerLog = new ObservableCollection<AnswerLogModel>();
            }
            _pickingIndex = 0;
            PickingTeam = SelectedRound.RoundStandings[_pickingIndex];
        }

        private void _gameTimer_Tick(object sender, object e)
        {
            //var asd = SelectedRound.TematicalTime.ToString();

            if (GamePhase == GamePhase.TEMATICAL)
            {
                SelectedRound.RoundSettings.TematicalTime = SelectedRound.RoundSettings.TematicalTime.Subtract(TimeSpan.FromSeconds(1));
                if (SelectedRound.RoundSettings.TematicalTime.TotalSeconds == 0)
                {
                    _gameTimer.Stop();
                }
            }
            else
            {
                SelectedRound.RoundSettings.LightningTime = SelectedRound.RoundSettings.LightningTime.Subtract(TimeSpan.FromSeconds(1));
                if (SelectedRound.RoundSettings.LightningTime.TotalSeconds == 0)
                {
                    _gameTimer.Stop();
                }
            }
        }


        private RoundModel _selectedRound;


        private ICommand _toggleTimerCommand;
        public ICommand ToggleTimerCommand => _toggleTimerCommand ??= new RelayCommand(ToggleTimer);

        //TODO: egységesíteni a neveket (game over vs round over)
        private ICommand _gameOverCommand;
        public ICommand GameOverCommand => _gameOverCommand ??= new RelayCommand(On_RoundOver);

        public event EventHandler<RoundModel> RoundOver;
        private void On_RoundOver()
        {
            SelectedRound.RoundEnded = true;
            //TODO create game history
            _gameTimer.Stop();
            IsMenuButtonVisible = false;
            HidePage?.Invoke(this, "Játék");
            RoundOver?.Invoke(this, SelectedRound);
        }

        private ICommand _skipAnswerCommand;
        public ICommand SkipAnswerCommand => _skipAnswerCommand ??= new RelayCommand(SkipAnswer);

        public void SkipAnswer()
        {
            if (CurrentQuestion != null)
            {
                AnswerText = string.Empty;
                CurrentQuestion = null;
                PickingTeam = SelectedRound.RoundStandings[++_pickingIndex % SelectedRound.RoundStandings.Count];
            }
        }

        private ICommand _submitAnswerCommand;
        public ICommand SubmitAnswerCommand => _submitAnswerCommand ??= new RelayCommand(SubmitAnser);

        private string _answerText;
        public string AnswerText
        {
            get => _answerText;
            set => SetProperty(ref _answerText, value);
        }

        private StandingDto _pickingTeam;
        public StandingDto PickingTeam
        {
            get => _pickingTeam;
            set => SetProperty(ref _pickingTeam, value);
        }
        private int _pickingIndex;
        private void SubmitAnser()
        {
            if (CurrentQuestion != null)
            {
                var gainedScore = AnswerText.ToLower() == CurrentQuestion.Answer.ToLower() ? CurrentQuestion.Value : -CurrentQuestion.Value;
                PickingTeam.Score += gainedScore;

                var answ = new AnswerLogModel
                {
                    TeamName = PickingTeam.Team.TeamName,
                    QuestionValue = gainedScore,
                };

                AnswerLog.Add(answ);
                PickingTeam = SelectedRound.RoundStandings[++_pickingIndex % SelectedRound.RoundStandings.Count];
                AnswerText = string.Empty;
                CurrentQuestion = null;
            }
        }

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
            dialog.Content = new CustomDialog("Biztosan továbblép a villámra?");

            if (await dialog.ShowAsync(ContentDialogPlacement.Popup) == ContentDialogResult.Primary)
            {
                GamePhase = GamePhase.LIGHTNING;
                //CurrentQuestion
            }
        }

        private ICommand _loadFromFileCommand;
        public ICommand LoadFromFileCommand => _loadFromFileCommand ??= new RelayCommand(LoadFromFileClicked);

        public async void LoadFromFileClicked()
        {
            FileOpenPicker picker = new FileOpenPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Window.Current);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();

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
            if ((GamePhase == GamePhase.TEMATICAL && SelectedRound.RoundSettings.TematicalTime.TotalSeconds == 0) ||
                (GamePhase == GamePhase.LIGHTNING && SelectedRound.RoundSettings.LightningTime.TotalSeconds == 0))
                return;

            if (_gameTimer.IsEnabled)
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


        private QuestionDto _currentQuestion;
        public QuestionSerieDto Serie => _mapper.Map<QuestionSerieDto>(DummyData.DummyData.QS1);

        private string _occasionName;
        public string OccasionName
        {
            get => _occasionName;
            set => SetProperty(ref _occasionName, value);
        }

        public QuestionDto CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                SetProperty(ref _currentQuestion, value);
            }
        }
        public RoundModel SelectedRound
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
