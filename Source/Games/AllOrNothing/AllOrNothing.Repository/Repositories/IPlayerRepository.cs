using AllOrNothing.Data;
using System.Collections.Generic;

namespace AllOrNothing.Repository
{
    public interface IPlayerRepository : IRepository<Player>
    {
        IEnumerable<Player> GetAllByInstitue(string Institue);
    }
}
