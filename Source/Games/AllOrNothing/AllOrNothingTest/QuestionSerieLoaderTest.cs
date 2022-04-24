using AllOrNothing.Data;
using AllOrNothing.Repository;
using AllOrNothing.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AllOrNothingTest
{
    public class QuestionSerieLoaderTest
    {

        private readonly QuestionSerieLoader _serieLoader;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionSerieLoaderTest()
        {
            _unitOfWork = new UnitOfWork(new AllOrNothingDbContext());
            _serieLoader = new QuestionSerieLoader(_unitOfWork);
        }

        private string ReadFileContent(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }

        #region Old Format Correct input
        [Fact]
        public void OldFormatCorrectInput1()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldCorrect", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_Correct.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Name.Should().Be("OldCorrect");
            serie.Id.Should().Be(0);

            serie.Topics.Count.Should().Be(5);
            serie.Topics.All(t => t.Questions.Count == 6).Should().BeTrue();
            serie.Topics[0].Name.Should().Be("Köves");
            serie.Topics[1].Name.Should().Be("Levegõs");
            serie.Topics[2].Name.Should().Be("Vizes");
            serie.Topics[3].Name.Should().Be("Csillagos");
            serie.Topics[4].Name.Should().Be("Népes");

            serie.Topics.All(t => t.Author == null).Should().BeTrue();
            serie.Topics.All(t => t.Competences == null).Should().BeTrue();
        }

        #endregion

        #region Old Format Incorrect input
        [Fact]
        public void OldFormateLessTopic()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldLessTopic", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_LessTopic.txt"), out serie);
            
            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormateMoreTopic()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldMoreTopic", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_MoreTopic.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormateLessQuestion()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldLessQuestion", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_LessQuestion.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormateMoreQuestion()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldMoreQuestion", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_MoreQuestion.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormateWrongDelimeter()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldFormatWrongDelimeter", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_WrongDelimeter.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }
        #endregion

        #region New Format Correct input

        [Fact]
        public void NewFormat_CheckTopics()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormatCorrect", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_Correct.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Name.Should().Be("NewFormatCorrect");
            serie.Topics.Count.Should().Be(5);
            serie.Topics.All(t => t.Questions.Count == 6).Should().BeTrue();
            List<int> compIds = new List<int>();
            foreach (var item in serie.Topics)
            {
                compIds = compIds.Union(item.Competences.Select(c => c.Id)).ToList();
            }
            compIds.Count().Should().Be(2);
        }

        [Fact]
        public void NewFormat_NoAuthorGiven()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormatNoAuthorGiven", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_NoAuthorGiven.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics.All(t => t.Author == null).Should().BeTrue();
        }

        [Fact]
        public void NewFormat_OnlyAuthorNameIsGiven()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_OnlyAuthorNameIsGiven", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_OnlyAuthorNameIsGiven.txt"), out serie);

            sucess.Should().BeTrue();
            foreach (var topic in serie.Topics)
            {
                topic.Author.Name.Should().Be("AuthorNameOnly");
                topic.Author.NickName.Should().BeNull();
                topic.Author.Institute.Should().BeNull();
            }
        }

        [Fact]
        public void NewFormat_OnlyAuthorIdIsGiven()
        {

        }

        [Fact]
        public void NewFormat_OnlyInstitueIsGiven()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_OnlyAuthorNameIsGiven", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_OnlyAuthorNameIsGiven.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics.All(t => t.Author == null).Should().BeTrue();
        }

        [Fact]
        public void NewFormat_NoPlayerWithTheAuthorsId()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void NewFormat_AuthorsNameNotMatchesPlayersNameWithTheGivenId()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region New Format InCorrect input

        #endregion
    }
}
