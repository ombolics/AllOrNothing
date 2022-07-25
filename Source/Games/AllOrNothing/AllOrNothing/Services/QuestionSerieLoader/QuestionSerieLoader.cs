using AllOrNothing.Data;
using AllOrNothing.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AllOrNothing.Services
{
    public class QuestionSerieLoader : QuestionSerieLoaderBase
    {
        #region Fields
        private IUnitOfWork _unitOfWork;
        private List<Competence> _allCompetences;
        private ILogger<QuestionSerieLoader> _logger;
        #endregion

        #region Constructors
        public QuestionSerieLoader(IUnitOfWork unitOfWork, ILogger<QuestionSerieLoader> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        #region Methods
        //mostly for testing porpusses
        public void SetupAllCompetences()
        {
            _allCompetences = _unitOfWork.Competences.GetAll().ToList();
        }

        public override List<QuestionSerie> LoadAllSeriesFromFolder(string folderPath, out string errorMessage)
        {
            errorMessage = "";
            SetupAllCompetences();
            var files = Directory.GetFiles(folderPath);
            var result = new List<QuestionSerie>();

            foreach (var file in files)
            {
                QuestionSerie serie;
                bool isSucessful = LoadFromTxt(file, out serie);
                if (isSucessful)
                {
                    result.Add(serie);
                }
                else
                {
                    var error = $"\t{file.Split(@"\").Last()}\n";
                    errorMessage += errorMessage == "" ? "Probléma lépett fel a következő fájlok beolvasása során:\n" + error : error;
                }
            }

            if (errorMessage != "")
            {
                errorMessage += "Ellenőrizze hogy nem sérültek-e a fájlok!";
            }
            return result;
        }
        public Topic ParseOldTopic(string data, string rowDelimeter)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            if (string.IsNullOrEmpty(rowDelimeter))
                throw new ArgumentException(nameof(rowDelimeter), "The row delimeter cannot be null or empty!");

            var lines = data.Split(rowDelimeter);
            Topic topic = new Topic()
            {
                Name = lines[0].Trim(),
                Author = null,
                Competences = null,
                Description = null,
            };

            for (int i = 1; i < lines.Length; i++)
            {
                topic.Questions.Add(new Question
                {
                    Text = lines[i],
                    Type = QuestionType.THEMATICAL,
                    Resource = null,
                    Answer = null,
                    ResourceType = QuestionResourceType.TEXT,
                    Value = i < 6 ? i * 1000 : 8000,
                });
            }
            return topic;
        }
        public override bool ParseOldFormat(string name, string content, out QuestionSerie value)
        {
            value = new QuestionSerie()
            {
                CreatedOn = DateTime.Now,
                Name = name.Trim(),
            };
            value.Topics = new List<Topic>();

            try
            {
                var topicArray = content.Split("\r\n\r\n");
                foreach (var pack in topicArray)
                {
                    var result = ParseOldTopic(pack, "\r\n");
                    if (null != result)
                        value.Topics.Add(result);
                }                                
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Error while parsing old serie format from file - {e.Message}");
                value = null;
                return false;
            }

            if (value.Topics.Count != 5 || !value.Topics.All(t => t.Questions.Count == 6))
            {
                value = null;
                return false;
            }
            return true;
        }

        public override Player ParseAuthor(string[] data)
        {
            Player author = null;
            int expectedId;
            if (int.TryParse(data[1], out expectedId))
            {
                author = _unitOfWork.Players.Get(expectedId);
            }

            if (author == null)
            {
                string name = data[3].Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    author = null;
                }
                else
                {
                    author = new Player
                    {
                        Name = name,
                        Institute = data[5].Trim(),
                    };
                }
            }
            return author;
        }
        public Topic ParseNewTopic(string data, string rowDelimeter)
        {
            var splitedWithDelimeter1 = data.Split(rowDelimeter);
            var line = splitedWithDelimeter1.Length < 2 ? data.Split("\r") : splitedWithDelimeter1;
            Topic topic = new Topic()
            {
                Name = line[0].Trim(),
                Competences = ParseCompetences(line[1]),
                Description = line[2].Trim(),
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
            return topic;
        }

        public override List<Competence> ParseCompetences(string row)
        {
            List<Competence> value = new List<Competence>();
            foreach (var item in row.Replace("(", "").Replace(")", "").Split(','))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                var competences = _allCompetences.Where(c => c.Name.ToLower().Contains(item.Trim().ToLower()));
                foreach (var competence in competences)
                {
                    if (!value.Contains(competence))
                    {
                        value.Add(competence);
                    }
                }
            }
            return value.Count == 0 ? null : value;
        }
        public override bool ParseNewFormat(string name, string content, out QuestionSerie value)
        {
            if (content == null || string.IsNullOrWhiteSpace(content))
            {
                value = null;
                return false;
            }

            const string sectionDelimeter = "\r\r\n";
            const string rowDelimeter = "\r\n";

            var splitedContent = content.Split(sectionDelimeter);
            value = new QuestionSerie()
            {
                CreatedOn = DateTime.Now,
                Name = name,
            };
            value.Topics = new List<Topic>();

            try
            {
                Player author = ParseAuthor(splitedContent[0].Split(rowDelimeter)[1].Split("\r"));
                foreach (var pack in splitedContent[1].Split("\r\n\r\n"))
                {
                    var topic = ParseNewTopic(pack, rowDelimeter);
                    topic.Author = author;
                    value.Topics.Add(topic);
                }
            }
            catch (Exception e)
            {
                value = null;
                return false;
            }

            if (value.Topics.Count != 5 || !value.Topics.All(t => t.Questions.Count == 6))
            {
                value = null;
                return false;
            }
            return true;
        }

        public override bool LoadFromTxt(string path, out QuestionSerie value)
        {
            bool hasError = false;
            value = null;
            try
            {
                var fullName = path.Split(@"\").Last();
                if (!fullName.EndsWith(".txt"))
                    return false;

                var name = fullName.Replace(".txt", "");
                var fileContent = File.ReadAllText(path, Encoding.UTF8);
                if (fileContent.Split("\r\n")[0].ToLower() != "szerző:")
                {
                    hasError = hasError || !ParseOldFormat(name, fileContent, out value);
                }
                else
                {
                    hasError = hasError || !ParseNewFormat(name, fileContent, out value);
                }
            }
            catch
            {
                hasError = true;
            }
            return !hasError;
        }
        #endregion 
    }
}
