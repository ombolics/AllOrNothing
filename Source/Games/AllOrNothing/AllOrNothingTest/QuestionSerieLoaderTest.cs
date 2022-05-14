using AllOrNothing.Data;
using AllOrNothing.Mapping;
using AllOrNothing.Repository;
using AllOrNothing.Services;
using AllOrNothingTest.Helpers;
using AutoMapper;
using CommunityToolkit.Mvvm.DependencyInjection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AllOrNothingTest
{
    public class QuestionSerieLoaderTest : IClassFixture<QuestoinSerieLoaderTestFixture>
    {

        private QuestoinSerieLoaderTestFixture _fixture;
        private readonly QuestionSerieLoader _serieLoader;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionSerieLoaderTest(QuestoinSerieLoaderTestFixture fixture)
        {
            _fixture = fixture;
            if (!fixture.Used)
            {
                Ioc.Default.ConfigureServices(ConfigureServices());
                fixture.Used = true;
            }
            _unitOfWork = Ioc.Default.GetService<IUnitOfWork>();
            _serieLoader = Ioc.Default.GetService<QuestionSerieLoader>();
            _serieLoader.SetupAllCompetences();
        }

        private System.IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<QuestionSerieLoader>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var dir = System.AppDomain.CurrentDomain.BaseDirectory;
            services.AddDbContext<AllOrNothingDbContext>(optionsBuilder => optionsBuilder.UseSqlite(@$"Data source={dir}..\..\..\..\AllOrNothing.Repository\AllOrNothingDb.db"));

            //Mapping

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services.BuildServiceProvider();
        }

        private string ReadFileContent(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }

        [Fact]
        public void WrongExtension()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("WrongExtension", ReadFileContent("TestFiles/QuestionSerieLoader/WrongExtension.json"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
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
        public void OldFormatLessTopic()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldLessTopic", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_LessTopic.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormatMoreTopic()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldMoreTopic", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_MoreTopic.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormatLessQuestion()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldLessQuestion", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_LessQuestion.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormatMoreQuestion()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseOldFormat("OldMoreQuestion", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_MoreQuestion.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void OldFormatWrongDelimeter()
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
                if (item.Competences == null)
                    continue;

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
                topic.Author.Institute.Should().Be("");
            }
        }

        [Fact]
        public void NewFormat_OnlyAuthorIdIsGiven()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_OnlyAuthorNameIsGiven", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_OnlyAuthorIdIsGiven.txt"), out serie);
            sucess.Should().BeTrue();

            var expectedAuthor = _unitOfWork.Players.Get(1);
            serie.Topics.All(t => t.Author.Id == expectedAuthor.Id &&
                                t.Author.Name == expectedAuthor.Name &&
                                t.Author.Institute == expectedAuthor.Institute).Should().BeTrue();
        }

        [Fact]
        public void NewFormat_NoPlayerWithOnlyIdGiven()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_NoPlayerWithOnlyIdGiven", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_NoPlayerWithOnlyIdGiven.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics.All(t => t.Author == null).Should().BeTrue();
        }

        [Fact]
        public void NewFormat_NameIsGivenIdIsGivenNoPlayerWithId()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_NameIsGivenIdIsGivenNoPlayerWithId", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_NameIsGivenIdIsGivenNoPlayerWithId.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics.All(t => t.Author.Id == default &&
                                  t.Author.Name == "TestName" &&
                                  t.Author.Institute == "APPLE").Should().BeTrue();
        }

        [Fact]
        public void NewFormat_AuthorsNameNotMatchesPlayersNameWithTheGivenId()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_AuthorsNameNotMatchesPlayersNameWithTheGivenId", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_AuthorsNameNotMatchesPlayersNameWithTheGivenId.txt"), out serie);
            sucess.Should().BeTrue();

            var expectedAuthor = _unitOfWork.Players.Get(1);
            serie.Topics.All(t => t.Author.Id == expectedAuthor.Id &&
                                t.Author.Name == expectedAuthor.Name &&
                                t.Author.Institute == expectedAuthor.Institute).Should().BeTrue();
        }

        [Fact]
        public void NewFormat_NoTopicDescription()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_NoTopicDescription", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_NoTopicDescription.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics[0].Description.Should().Be("()");
            serie.Topics[1].Description.Should().Be("()");
        }

        [Fact]
        public void NewFormat_NoCompetenceIsGiven()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_NoCompetenceIsGiven", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_NoCompetenceIsGiven.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics.All(t => t.Competences == null).Should().BeTrue();
        }

        [Fact]
        public void NewFormat_DataBaseNotContainsTheGivenCompetence()
        {
            _serieLoader.SetupAllCompetences();
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_DataBaseNotContainsTheGivenCompetence", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_DataBaseNotContainsTheGivenCompetence.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics[0].Competences.Count.Should().Be(1);
            serie.Topics[0].Competences[0].Name.ToLower().Should().Be("történelem");
            for (int i = 1; i < serie.Topics.Count; i++)
            {
                serie.Topics[i].Competences.Should().BeNull();
            }
        }

        [Fact]
        public void NewFormat_SameCompetenceIsGivenTwiceAtATopic()
        {

            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_SameCompetenceIsGivenTwiceAtATopic", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_SameCompetenceIsGivenTwiceAtATopic.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Topics[0].Competences.Count.Should().Be(1);
            serie.Topics[1].Competences.Count.Should().Be(1);
            serie.Topics[2].Competences.Count.Should().Be(1);
        }
        #endregion

        #region New Format InCorrect input
        [Fact]
        public void NewFormat_LessTopic()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_MoreTopic", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_MoreTopic.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void NewFormat_MoreTopic()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_MoreTopic", ReadFileContent("TestFiles/QuestionSerieLoader/OldFormat_MoreTopic.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void NewFormat_MoreQuestion()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_MoreQuestion", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_MoreQuestion.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void NewFormat_LessQuestion()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_LessQuestion", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_LessQuestion.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void NewFormat_WrongDelimeter()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_WrongDelimeter", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_WrongDelimeter.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void NewFormat_DescriptionIsMissing()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_DescriptionIsMissing", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_DescriptionIsMissing.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void NewFormat_CompetenceRowIsMissing()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_CompetenceRowIsMissing", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_CompetenceRowIsMissing.txt"), out serie);

            sucess.Should().BeFalse();
            serie.Should().BeNull();
        }

        [Fact]
        public void NewFormat_WrongCompetenceDelimeter()
        {
            QuestionSerie serie;
            bool sucess = _serieLoader.ParseNewFormat("NewFormat_WrongCompetenceDelimeter", ReadFileContent("TestFiles/QuestionSerieLoader/NewFormat_WrongCompetenceDelimeter.txt"), out serie);

            sucess.Should().BeTrue();
            serie.Should().NotBeNull();
        }
        #endregion
    }
}
