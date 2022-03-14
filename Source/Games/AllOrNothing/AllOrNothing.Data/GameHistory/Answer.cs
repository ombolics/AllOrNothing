using System;

namespace AllOrNothing.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public TimeSpan GameTime { get; set; }
        public bool IsGoodAnswer { get; set; }
        public Team Responder { get; set; }
        public DateTime Date { get; set; }
    }
}
