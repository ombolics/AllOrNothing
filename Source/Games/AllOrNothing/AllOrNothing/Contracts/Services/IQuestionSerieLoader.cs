using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
