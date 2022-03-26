using AllOrNothing.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllOrNothing.Repository.Repositories
{
    public class QuestionSerieRepository : Repository<QuestionSerie>, IQuestionSerieRepository
    {
        public QuestionSerieRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<QuestionSerie> GetAllAvaibleByAuthor(int authorId)
        {
            //return Context.Set<QuestionSerie>()
            //    .Where(q => q.Authors
            //        .Where(p => p.Id == authorId)
            //        .ToList().Count > 0)
            //    .ToList();
            return null;
        }

        public IEnumerable<QuestionSerie> GetAllAvaibleByAuthorInstitute(int instituteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionSerie> GetAllAvaibleByCompetence(int topicId)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<QuestionSerie> GetAll()
        {
            return Context.Set<QuestionSerie>()
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Questions)
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Competences)
                .ToList();
        }

        public IEnumerable<QuestionSerie> GetAllAvaible()
        {
            return Context.Set<QuestionSerie>()
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Questions)
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Competences)
                .Where(q => !q.IsDeleted)
                .ToList();
        }
    }
}
