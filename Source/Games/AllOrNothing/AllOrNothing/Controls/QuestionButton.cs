using AllOrNothing.AutoMapper.Dto;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Controls
{
    class QuestionButton : Button
    {
        private QuestionDto _question;
        public QuestionDto Question
        {
            get => _question;
            set => _question = value;
        }

        public QuestionButton(QuestionDto question) : base()
        {
            _question = question;
        }
    }
}
