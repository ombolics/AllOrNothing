using AllOrNothing.Data;

namespace AllOrNothing.Controls
{
    public class QuestionPickedEventArgs
    {
        public QuestionPickedEventArgs(Question question)
        {
            Question = question;
        }

        public Question Question { get; set; }
    }
}