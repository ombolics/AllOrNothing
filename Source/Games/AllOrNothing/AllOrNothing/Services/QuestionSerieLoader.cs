using AllOrNothing.Data;
using System.Collections.Generic;
using System.IO;

namespace AllOrNothing.Services
{
    public class QuestionSerieLoader
    {
        public QuestionSerieLoader()
        {

        }
        public List<QuestionSerie> LoadAllSeriesFromFolder(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            var result = new List<QuestionSerie>();
            foreach (var file in files)
            {
                result.Add(LoadFromTxt(file));
            }
            return result;
        }

        public QuestionSerie LoadFromTxt(string path)
        {
            StreamReader sr = new StreamReader(path);
            QuestionSerie value = new QuestionSerie();
            value.Topics = new List<Topic>();
            while (!sr.EndOfStream)
            {

                for (int i = 0; i < 5; i++)
                {
                    Topic t = new Topic();
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
                    sr.ReadLine();
                    value.Topics.Add(t);
                }

            }

            return value;
        }
    }
}
