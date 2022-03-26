using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Data.DataExtensions
{
    public static class TopicExtensions
    {
        public static void SetValue(this Topic originalTopic, Topic newTopic)
        {
            originalTopic.Id = newTopic.Id;
            originalTopic.Name = newTopic.Name;
            for (int i = 0; i < originalTopic.Questions.Count; i++)
            {
                originalTopic.Questions[i].SetValue(newTopic.Questions[i]);
            }
        }
    }
}
