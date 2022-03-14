﻿using AllOrNothing.Repository.Repositories;

namespace AllOrNothing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AllOrNothingDbContext _context;
        public UnitOfWork(AllOrNothingDbContext context)
        {
            _context = context;
            Players = new PlayerRepository(context);
            QuestionSeries = new QuestionSerieRepository(context);
        }

        public IPlayerRepository Players
        {
            get;
            private set;
        }
        public IQuestionSerieRepository QuestionSeries
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
