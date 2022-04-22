using AllOrNothing.Repository;
using AllOrNothing.Services;
using System;
using System.IO;
using Xunit;

namespace AllOrNothingTest
{
    public class QuestionSerieLoaderTest
    {

        private readonly QuestionSerieLoader _serieLoader;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionSerieLoaderTest()
        {
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
            var serie = _serieLoader.LoadOldFormatFromTxt("OldCorrect", ReadFileContent("TestFiles/QuestionSerieFolder/OldFormat_Correct.txt"));

            
        }

        #endregion

        #region Old Format Incorrect input
        [Fact]
        public void OldFormateLessTopic()
        {

        }

        [Fact]
        public void OldFormateMoreTopic()
        {

        }

        [Fact]
        public void OldFormateLessQuestion()
        {

        }

        [Fact]
        public void OldFormateMoreQuestion()
        {

        }
        [Fact]
        public void OldFormateWrongDelimeter()
        {

        }
        #endregion
    }
}
