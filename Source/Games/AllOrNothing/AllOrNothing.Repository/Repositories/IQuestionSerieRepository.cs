using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Repository.Repositories
{
    public interface IQuestionSerieRepository : IRepository<QuestionSerie>
    {
        IEnumerable<QuestionSerie> GetAllByCompetence(int topicId);
        IEnumerable<QuestionSerie> GetAllByAuthor(int authorId);
        IEnumerable<QuestionSerie> GetAllByAuthorInstitute(int instituteId);
    }
}
