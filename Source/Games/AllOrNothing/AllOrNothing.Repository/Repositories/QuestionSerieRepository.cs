using AllOrNothing.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Repository.Repositories
{
    public class QuestionSerieRepository : Repository<QuestionSerie>, IQuestionSerieRepository
    {
        public QuestionSerieRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<QuestionSerie> GetAllByAuthor(int authorId)
        {
            return Context.Set<QuestionSerie>()
                .Where(q => q.Authors
                    .Where(p => p.Id == authorId)
                    .ToList().Count > 0)
                .ToList();
        }

        public IEnumerable<QuestionSerie> GetAllByAuthorInstitute(int instituteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionSerie> GetAllByCompetence(int topicId)
        {
            throw new NotImplementedException();
        }
    }
}
