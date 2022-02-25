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

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingGameViewModel : ObservableRecipient
    {

        public AllOrNothingGameViewModel()
        {
            ScoreTest = 3000;
            IsRoundSettingsVisible = true;
            IsTematicalVisible = false;
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += _gameTimer_Tick;
        }

        public void RoundSelected(object sender, ItemClickEventArgs e)
        {
            SelectedRound = e.ClickedItem as RoundSettingsModel;
        }

        private void _gameTimer_Tick(object sender, object e)
        {
            //var asd = SelectedRound.TematicalTime.ToString();
            
            if(IsTematicalVisible)
            {
                if(SelectedRound.TematicalTime.TotalSeconds == 0)
                {
                    _gameTimer.Stop();
                }
                SelectedRound.TematicalTime = SelectedRound.TematicalTime.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                if (SelectedRound.LightningTime.TotalSeconds == 0)
                {
                    _gameTimer.Stop();
                }
                SelectedRound.LightningTime = SelectedRound.LightningTime.Subtract(TimeSpan.FromSeconds(1));
            }
        }

        private bool _isTematicalVisible;

        private RoundSettingsModel _selectedRound;

        

        private ICommand _toggleTimerCommand;

        public ICommand ToggleTimerCommand => _toggleTimerCommand ??= new RelayCommand(ToggleTimer);

        private DispatcherTimer _gameTimer;


        public ObservableCollection<QuestionSerie> TestSeries => new ObservableCollection<QuestionSerie> { DummyData.DummyData.QS1 };

        private void ToggleTimer()
        {
            if ( (SelectedRound.IsTematicalAllowed && SelectedRound.TematicalTime.TotalSeconds == 0) ||
                (SelectedRound.IsLightningAllowed && SelectedRound.LightningTime.TotalSeconds == 0))
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

        private ICommand _startGameCommand;

        public ICommand StartGameCommand => _startGameCommand ??= new RelayCommand(On_StartGame);

        private void On_StartGame()
        {
            IsRoundSettingsVisible = false;
            IsTematicalVisible = true;
        }

        private Question _currentQuestion;
        public QuestionSerie Serie => QuestionSerieDummyData.QS1;

        private bool _isRoundSettingsVisible;

        public Question CurrentQuestion 
        {
            get => _currentQuestion;
            set
            {
                SetProperty(ref _currentQuestion, value);
            } 
        }

        public bool IsRoundSettingsVisible 
        { 
            get => _isRoundSettingsVisible;
            set => SetProperty(ref _isRoundSettingsVisible, value); 
        }
        public bool IsTematicalVisible 
        { 
            get => _isTematicalVisible; 
            set => SetProperty(ref _isTematicalVisible, value); 
        }
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
                SelectedRound = _rounds[0];
            }
        }

        private ObservableCollection<RoundSettingsModel> _rounds;
    }
}
