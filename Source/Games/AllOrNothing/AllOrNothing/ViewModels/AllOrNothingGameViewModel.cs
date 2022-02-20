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

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingGameViewModel : ObservableRecipient
    {

        public AllOrNothingGameViewModel()
        {
            ScoreTest = 3000;
            IsRoundSettingsVisible = true;
            IsTematicalVisible = false;
        }

        private bool _isTematicalVisible;

       

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
    }
}
