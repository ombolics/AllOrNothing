using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Models
{
    public class AnswerLogModel
    {
        public string TopicName;
        public int QuestionValue;    
        public string TeamName;

        public override string ToString()
        {
            return $"{TeamName}\t{TopicName}\t{QuestionValue}";
        }
    }
}
