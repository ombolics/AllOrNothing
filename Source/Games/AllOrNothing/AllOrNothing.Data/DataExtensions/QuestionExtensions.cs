using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Data.DataExtensions
{
    public static class QuestionExtensions
    {
        public static void SetValue(this Question originalQuestion, Question newQuestion)
        {
            originalQuestion.Id = newQuestion.Id;
            originalQuestion.Resource = newQuestion.Resource;
            originalQuestion.ResourceType = newQuestion.ResourceType;
            originalQuestion.Text = newQuestion.Text;
            originalQuestion.Type = newQuestion.Type;
            originalQuestion.Value = newQuestion.Value;
        }
    }
}
