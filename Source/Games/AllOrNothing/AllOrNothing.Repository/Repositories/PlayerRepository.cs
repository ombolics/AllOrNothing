using AllOrNothing.Data;
using System.Collections.Generic;
using System.Linq;

namespace AllOrNothing.Repository
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(IAllOrNothingDbContext context) : base(context as AllOrNothingDbContext)
        {
        }

        public IAllOrNothingDbContext AllOrNothingDbContext => Context as AllOrNothingDbContext;

        public IEnumerable<Player> GetAllAvaible()
        {
            return Context.Set<Player>()
                .Where(p => !p.IsDeleted)
                .ToList(); ;
        }

        public IEnumerable<Player> GetAllAvaibleByInstitue(string Institue)
        {
            return Context.Set<Player>()
                .Where(p => !p.IsDeleted && p.Institute == Institue)
                .ToList();
        }
    }
}
