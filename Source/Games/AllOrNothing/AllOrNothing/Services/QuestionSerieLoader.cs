using AllOrNothing.Data;
using AllOrNothing.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AllOrNothing.Services
{
    public class QuestionSerieLoader
    {
        public QuestionSerieLoader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork;
        private List<Competence> _allCompetences;
        public List<QuestionSerie> LoadAllSeriesFromFolder(string folderPath, out string errorMessage)
        {
            errorMessage = "";
            _allCompetences = _unitOfWork.Competences.GetAll().ToList();
            var files = Directory.GetFiles(folderPath);
            var result = new List<QuestionSerie>();

            foreach (var file in files)
            {
                string error = "";
                var serie = LoadFromTxt(file, out error);
                if (error != "")
                {
                    errorMessage += errorMessage == "" ? "Probléma lépett fel a következő fájlok beolvasása során:\n" + error : error;
                }
                if (serie != null)
                    result.Add(serie);
            }
            if (errorMessage != "")
            {
                errorMessage += "Ellenőrizze hogy nem sérültek-e a fájlok!";
            }
            return result;
        }

        public QuestionSerie LoadOldFormatFromTxt(string name, string content)
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

            return value;
        }

        public QuestionSerie LoadNewFormatFromTxt(string name, string content)
        {
            if (content == null || string.IsNullOrWhiteSpace(content))
                return null;

            var splitedContent = content.Split("\r\r\n");
            QuestionSerie value = new QuestionSerie()
            {
                CreatedOn = DateTime.Now,
                Name = name,
            };
            value.Topics = new List<Topic>();
            var authorData = splitedContent[0].Split("\r\n")[1].Split("\r");

            var author = _unitOfWork.Players.Get(int.Parse(authorData[1]));
            if (author == null)
            {
                author = new Player
                {
                    Name = authorData[3],
                    Institute = authorData[5],
                };
            }

            foreach (var pack in splitedContent[1].Split("\r\n\r\n"))
            {
                var splitedWithDelimeter1 = pack.Split("\r\n");
                var line = splitedWithDelimeter1.Length < 2 ? pack.Split("\r") : splitedWithDelimeter1;

                var competences = new List<Competence>();
                foreach (var item in line[1].Replace("(", "").Replace(")", "").Split(','))
                {
                    competences.AddRange(_allCompetences.Where(c => c.Name.Contains(item.Trim())).ToList());
                }
                Topic topic = new Topic()
                {
                    Name = line[0],
                    Author = author,
                    Competences = competences,
                    Description = line[2],
                };

                int questionCounter = 1;
                for (int i = 3; i < line.Length - 1; i += 2)
                {
                    topic.Questions.Add(new Question
                    {
                        Text = line[i],
                        Type = QuestionType.THEMATICAL,
                        Resource = null,
                        Answer = line[i + 1],
                        ResourceType = QuestionResourceType.TEXT,
                        Value = questionCounter < 6 ? questionCounter * 1000 : 8000,
                    });
                    questionCounter++;
                }
                value.Topics.Add(topic);
            }
            return value;

        }



        public QuestionSerie LoadFromTxt(string path, out string errorMessage)
        {
            errorMessage = "";
            QuestionSerie value = null;


            var fullName = path.Split(@"\").Last();
            //if (!fullName.EndsWith(".txt"))
            //{
            //    throw new ArgumentException();
            //}
            var name = fullName.Replace(".txt", "");
            var fileContent = File.ReadAllText(path, Encoding.UTF8);

            if (fileContent.Split("\r\n")[0].ToLower() != "szerző:")
            {
                value = LoadOldFormatFromTxt(name, fileContent);
            }
            else
            {
                value = LoadNewFormatFromTxt(name, fileContent);
            }

            try
            {

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                var splited = path.Split($@"\");
                errorMessage += $"\t{splited[splited.Length - 1]}\n";
            }
            return value;
        }
    }
}
