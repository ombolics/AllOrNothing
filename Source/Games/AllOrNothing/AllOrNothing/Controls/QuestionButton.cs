﻿using AllOrNothing.Mapping;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Controls
{
    class QuestionButton : Button

    {
        public QuestionButton()
        {
             
        }
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
