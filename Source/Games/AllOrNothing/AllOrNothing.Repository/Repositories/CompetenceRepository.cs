using AllOrNothing.Data;
using AllOrNothing.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AllOrNothing.Repository.Repositories
{
    public class CompetenceRepository : Repository<Competence>, ICompetenceRepository
    {
        public CompetenceRepository(DbContext context) : base(context)
        {

        }
    }
}
