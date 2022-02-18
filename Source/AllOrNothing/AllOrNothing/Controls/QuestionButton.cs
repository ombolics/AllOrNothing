using Microsoft.UI.Xaml.Controls;
using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Controls
{
    class QuestionButton : Button
    {
        private Question _question;
        public Question Question
        {
            get => _question;
            set => _question = value;
        }

        public QuestionButton(Question question) : base()
        {
            _question = question;
        }
    }
}
