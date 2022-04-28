using CommunityToolkit.Mvvm.ComponentModel;

namespace AllOrNothing.Models
{
    public class AnswerLogModel : ObservableRecipient
    {
        #region Fields
        private int _questionValue;
        private string _teamName;
        #endregion

        #region Properties
        public int QuestionValue
        {
            get => _questionValue;
            set => SetProperty(ref _questionValue, value);
        }

        public string TeamName
        {
            get => _teamName;
            set => SetProperty(ref _teamName, value);
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{TeamName}\t\t{QuestionValue}";
        }
        #endregion
    }
}
