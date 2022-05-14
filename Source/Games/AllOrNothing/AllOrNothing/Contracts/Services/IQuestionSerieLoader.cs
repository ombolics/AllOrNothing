using AllOrNothing.Data;
using System.Collections.Generic;

namespace AllOrNothing.Contracts.Services
{
    public interface IQuestionSerieLoader
    {
        bool ParseNewFormat(string name, string content, out QuestionSerie value);
        List<QuestionSerie> LoadAllSeriesFromFolder(string folderPath, out string errorMessage);
        bool ParseOldFormat(string name, string content, out QuestionSerie value);
        bool LoadFromTxt(string path, out QuestionSerie value);
    }
}
