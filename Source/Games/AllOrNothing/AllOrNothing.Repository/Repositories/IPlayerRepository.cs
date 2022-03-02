using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Repository
{
    public interface IPlayerRepository : IRepository<Player>
    {
        IEnumerable<Player> GetAllByInstitue(string Institue);
    }
}
