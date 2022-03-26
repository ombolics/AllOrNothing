using AllOrNothing.Data;
using System.Collections.Generic;

namespace AllOrNothing.Repository.Contracts
{
    public interface IQuestionSerieRepository : IRepository<QuestionSerie>
    {
        IEnumerable<QuestionSerie> GetAllAvaibleByCompetence(int topicId);
        IEnumerable<QuestionSerie> GetAllAvaibleByAuthor(int authorId);
        IEnumerable<QuestionSerie> GetAllAvaibleByAuthorInstitute(int instituteId);
        IEnumerable<QuestionSerie> GetAllAvaible();
    }
}
