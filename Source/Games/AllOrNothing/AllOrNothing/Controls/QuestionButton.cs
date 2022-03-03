using Microsoft.UI.Xaml.Controls;
using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllOrNothing.AutoMapper.Dto;

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
