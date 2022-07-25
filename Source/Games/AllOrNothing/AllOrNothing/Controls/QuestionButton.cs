using AllOrNothing.Mapping;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Controls
{
    class QuestionButton : Button
    {
        public QuestionButton()
        {
        }
        private string _topicName;
        private QuestionDto _question;
        public QuestionDto Question
        {
            get => _question;
            set => _question = value;
        }

        public string TopicName { get; set; }
        public QuestionButton(QuestionDto question, string topicName) : base()
        {
            _question = question;
            _topicName = topicName;
        }
    }
}
