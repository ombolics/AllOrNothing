using AllOrNothing.Data;
using System.Collections.Generic;

namespace AllOrNothing.Repository.Contracts
{
    public interface IPlayerRepository : IRepository<Player>
    {
        IEnumerable<Player> GetAllAvaibleByInstitue(string Institue);
        IEnumerable<Player> GetAllAvaible();
    }
}
