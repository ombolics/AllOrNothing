using AllOrNothing.Contracts.Services;
using AllOrNothing.Data;
using System.Collections.Generic;

namespace AllOrNothing.Services
{
    public abstract class QuestionSerieLoaderBase : IQuestionSerieLoader
    {
        public abstract List<QuestionSerie> LoadAllSeriesFromFolder(string folderPath, out string errorMessage);
        public abstract bool LoadFromTxt(string path, out QuestionSerie value);
        public abstract bool ParseNewFormat(string name, string content, out QuestionSerie value);
        public abstract bool ParseOldFormat(string name, string content, out QuestionSerie value);
        public abstract Player ParseAuthor(string[] data);
        public abstract List<Competence> ParseCompetences(string row);
    }
}
