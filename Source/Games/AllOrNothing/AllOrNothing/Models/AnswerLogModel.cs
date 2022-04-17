using CommunityToolkit.Mvvm.ComponentModel;

namespace AllOrNothing.Models
{
    public class AnswerLogModel : ObservableRecipient
    {
        private int _questionValue;
        public int QuestionValue
        {
            get => _questionValue;
            set => SetProperty(ref _questionValue, value);
        }
        private string _teamName;
        public string TeamName
        {
            get => _teamName;
            set => SetProperty(ref _teamName, value);
        }

        public override string ToString()
        {
            return $"{TeamName}\t\t{QuestionValue}";
        }
    }
}
