using AllOrNothing.Data;
using System.Collections.Generic;

namespace AllOrNothing.Repository.Repositories
{
    public interface IQuestionSerieRepository : IRepository<QuestionSerie>
    {
        IEnumerable<QuestionSerie> GetAllByCompetence(int topicId);
        IEnumerable<QuestionSerie> GetAllByAuthor(int authorId);
        IEnumerable<QuestionSerie> GetAllByAuthorInstitute(int instituteId);
    }
}
