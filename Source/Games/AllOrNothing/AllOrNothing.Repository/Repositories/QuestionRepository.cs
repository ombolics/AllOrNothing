using AllOrNothing.Data;
using AllOrNothing.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AllOrNothing.Repository.Repositories
{
    class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(DbContext context) : base(context)
        {

        }
    }
}
