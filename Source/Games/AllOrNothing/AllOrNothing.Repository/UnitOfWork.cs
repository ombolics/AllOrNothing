using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AllOrNothingDbContext _context;
        public UnitOfWork(AllOrNothingDbContext context)
        {
            _context = context;
            Players = new PlayerRepository(context);
        }

        public IPlayerRepository Players
        {
            get;
            private set;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
