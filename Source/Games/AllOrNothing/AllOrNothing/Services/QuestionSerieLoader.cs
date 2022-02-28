using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Services
{
    public class QuestionSerieLoader
    {
        public QuestionSerieLoader()
        {

        }

        public  QuestionSerie LoadFromTxt(string path)
        {
            StreamReader sr = new StreamReader(path);
            QuestionSerie value = new QuestionSerie();
            while(!sr.EndOfStream)
            {
                Topic t = new Topic();
                for (int i = 0; i < 5; i++)
                {
                    t.Name = sr.ReadLine();
                    t.Questions = new List<Question>();

                    for (int j = 0; j < 6; j++)
                    {
                        t.Questions.Add(new Question
                        {
                            Text = sr.ReadLine(),
                            Type = QuestionType.THEMATICAL,
                            ResourceType = QuestionResourceType.TEXT,
                        });
                    }

                }
                value.Topics.Add(t);
            }

            return value;
        }
    }
}
