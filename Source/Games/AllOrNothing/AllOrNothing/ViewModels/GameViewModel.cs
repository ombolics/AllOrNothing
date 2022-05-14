using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;
using AllOrNothing.Mapping;
using AllOrNothing.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Windows.Storage.Pickers;

namespace AllOrNothing.ViewModels
{
    public enum GamePhase
    {
        TEMATICAL,
        LIGHTNING
    }
    //TODO: egységesíteni a neveket (game over vs round over)
    public class GameViewModel : ViewModelBase
    {
        #region Fields
        private GamePhase _gamePhase;
        private ObservableCollection<AnswerLogModel> _answerLog;
        private RoundModel _selectedRound;

        private ICommand _toggleTimerCommand;
        private ICommand _gameOverCommand;
        private ICommand _skipAnswerCommand;
        private ICommand _submitAnswerCommand;
        private ICommand _showLightningCommand;

        private string _answerText;
        private StandingDto _pickingTeam;
        private int _pickingIndex;
        private DispatcherTimer _gameTimer;
        private XamlRoot _pageXamlRoot;
        private QuestionDto _currentQuestion;
        private string _occasionName;
        private bool _lightningButtonVisible;
        #endregion

        #region Constructors
        public GameViewModel(INavigationViewService navigationViewService)
            : base(navigationViewService)
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += _gameTimer_Tick;
            ReachablePages = new List<Type>();
        }
        #endregion

        #region Events
        public event EventHandler<RoundModel> RoundOver;
        #endregion

        #region Properties
        public ObservableCollection<AnswerLogModel> AnswerLog
        {
            get => _answerLog;
            set => SetProperty(ref _answerLog, value);
        }
        public string AnswerText
        {
            get => _answerText;
            set => SetProperty(ref _answerText, value);
        }
        public StandingDto PickingTeam
        {
            get => _pickingTeam;
            set => SetProperty(ref _pickingTeam, value);
        }
        public XamlRoot PageXamlRoot
        {
            get => _pageXamlRoot;
            set => SetProperty(ref _pageXamlRoot, value);
        }
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
                if (GamePhase == GamePhase.LIGHTNING && value == null)
                {
                    value = new QuestionDto
                    {
                        Value = 3000,
                    };
                }
                Debug.WriteLine("VM\t" + value?.GetHashCode());
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
        public bool LightningButtonVisible
        {
            get => _lightningButtonVisible;
            set => SetProperty(ref _lightningButtonVisible, value);
        }

        public ICommand ToggleTimerCommand => _toggleTimerCommand ??= new RelayCommand(ToggleTimer);
        public ICommand GameOverCommand => _gameOverCommand ??= new RelayCommand(On_RoundOver);
        public ICommand SkipAnswerCommand => _skipAnswerCommand ??= new RelayCommand(SkipAnswer);
        public ICommand SubmitAnswerCommand => _submitAnswerCommand ??= new RelayCommand(SubmitAnser);
        public ICommand ShowLightningCommand => _showLightningCommand ??= new RelayCommand(ShowLightning);

        #endregion

        #region Methods
        public void SetupRound(RoundModel m)
        {
            SelectedRound = m;

            if (SelectedRound.RoundSettings.IsTematicalAllowed)
                SetupTematical();
            else
                SetupLightning();

            if (SelectedRound.RoundSettings.IsGameWithoutButtonsEnabled)
            {
                AnswerLog = new ObservableCollection<AnswerLogModel>();
            }
            _pickingIndex = 0;
            PickingTeam = SelectedRound.RoundStandings[_pickingIndex];
        }
        public void SkipAnswer()
        {
            if (CurrentQuestion != null)
            {
                AnswerText = string.Empty;
                CurrentQuestion = null;
                PickingTeam = SelectedRound.RoundStandings[++_pickingIndex % SelectedRound.RoundStandings.Count];
            }
        }
        private void SetupLightning()
        {
            SelectedRound.RoundSettings.LightningTime = SelectedRound.RoundSettings.LightningTime.ShiftToRight();
            GamePhase = GamePhase.LIGHTNING;
            LightningButtonVisible = false;
            CurrentQuestion = new QuestionDto
            {
                Value = 3000,
            };
        }

        private void SetupTematical()
        {
            GamePhase = GamePhase.TEMATICAL;
            LightningButtonVisible = SelectedRound.RoundSettings.IsLightningAllowed;
            SelectedRound.RoundSettings.TematicalTime = SelectedRound.RoundSettings.TematicalTime.ShiftToRight();
        }

        private void _gameTimer_Tick(object sender, object e)
        {
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

        private async void On_RoundOver()
        {
            string popUpContent = "Biztosan véget vet a körnek?";
            if (GamePhase == GamePhase.TEMATICAL && SelectedRound.RoundSettings.IsLightningAllowed)
            {
                popUpContent += "\nA villámkérdések még visszavannak!";
            }

            if (await PopupManager.ShowDialog(PageXamlRoot, "Kör vége?", popUpContent, ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                SelectedRound.RoundEnded = true;
                //TODO create game history
                _gameTimer.Stop();
                IsMenuButtonVisible = false;
                RoundOver?.Invoke(this, SelectedRound);
            }
        }

        private void SubmitAnser()
        {
            if (CurrentQuestion == null)
                return;

            int gainedScore;
            if (AnswerText?.ToLower() == CurrentQuestion.Answer?.ToLower())
            {
                PopupManager.ShowDialog(PageXamlRoot, "Helyes válasz!", $"{CurrentQuestion.Answer}", ContentDialogButton.Close, "", "Ezaz!");
                gainedScore = CurrentQuestion.Value;
            }
            else
            {
                PopupManager.ShowDialog(PageXamlRoot, "Rossz válasz!", $"A helyes válasz:\n{CurrentQuestion.Answer}", ContentDialogButton.Close, "", "A fenébe!");
                gainedScore = -CurrentQuestion.Value;
            }

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

        private async void ShowLightning()
        {
            if (await PopupManager.ShowDialog(PageXamlRoot, "Villám?", "Biztosan továbblép a villámkérdésekhez?", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                SetupLightning();
            }
        }

        public async void LoadFromFileClicked()
        {
            FileOpenPicker picker = new FileOpenPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Window.Current);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();

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
        #endregion
    }
}
