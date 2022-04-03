using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public QuestionSerie LoadOldFormatFromTxt(string name, string content = null)
        {
            QuestionSerie value = new QuestionSerie()
            {
                CreatedOn = DateTime.Now,
                Name = name,
                
                
            };
            value.Topics = new List<Topic>();
            var topicArray = content.Split("\r\n\r\n");
            foreach (var pack in topicArray)
            {
                var line = pack.Split("\r\n");
                Topic topic = new Topic()
                {
                    Name = line[0],
                    Author = null,
                    Competences = null,
                    Description = null,
                };

                for (int i = 1; i < line.Length; i++)
                {
                    topic.Questions.Add(new Question
                    {
                        Text = line[i],
                        Type = QuestionType.THEMATICAL,
                        Resource = null,
                        Answer = null,
                        ResourceType = QuestionResourceType.TEXT,
                        Value = i < 6 ? i * 1000 : 8000,
                    });
                }
                value.Topics.Add(topic);
            }
            //StreamReader sr = new StreamReader(new Stream())
            //value.Topics = new List<Topic>();
            //while (!sr.EndOfStream)
            //{

            //    for (int i = 0; i < 5; i++)
            //    {
            //        Topic t = new Topic();
            //        t.Name = sr.ReadLine();
            //        t.Questions = new List<Question>();

            //        for (int j = 0; j < 6; j++)
            //        {
            //            t.Questions.Add(new Question
            //            {
            //                Text = sr.ReadLine(),
            //                Type = QuestionType.THEMATICAL,
            //                ResourceType = QuestionResourceType.TEXT,
            //            });
            //        }
            //        sr.ReadLine();
            //        value.Topics.Add(t);
            //    }

            //}

            return value;
        }

        public QuestionSerie LoadNewFormatFromTxt(string path = null, string content = null)
        {
            throw new NotImplementedException();
        }

        public QuestionSerie LoadFromTxt(string path)
        {
            var fileContent = File.ReadAllText(path, Encoding.UTF8);
            var name = path.Split(@"\").Last().Replace(".txt","");
            if (fileContent.Split("\r\n")[0].Split(':')[0].ToLower() != "szerző")
            {
                return LoadOldFormatFromTxt(name, fileContent);
            }
            else
            {
                return LoadNewFormatFromTxt(null, fileContent);
            }           
        }
    }
}
