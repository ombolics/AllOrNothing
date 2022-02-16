using CommunityToolkit.Mvvm.ComponentModel;
using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllOrNothing.DummyData;

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingTematicalViewModel : ObservableRecipient
    {

        #region Singleton pattern

        private static AllOrNothingTematicalViewModel _instance = null;
        private static readonly object padlock = new object();

        private AllOrNothingTematicalViewModel()
        {
            ScoreTest = 3000;
        }

        public static AllOrNothingTematicalViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AllOrNothingTematicalViewModel();
                    }
                    return _instance;
                }
            }
        }
        #endregion 

        private int _scoreTest;
        public int ScoreTest 
        {
            get => _scoreTest;
            set => SetProperty(ref _scoreTest, value);
        }

        private Question _currentQuestion;
        public QuestionSerie Serie => QuestionSerieDummyData.QS1;

        public Question CurrentQuestion 
        {
            get => _currentQuestion;
            set
            {
                SetProperty(ref _currentQuestion, value);
            } 
        }
    }
}
