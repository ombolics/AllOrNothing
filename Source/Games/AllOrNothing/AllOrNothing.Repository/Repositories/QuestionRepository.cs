using AllOrNothing.Data;
using AllOrNothing.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Repository.Repositories
{
    class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(DbContext context) : base(context)
        {

        }
    }
}
