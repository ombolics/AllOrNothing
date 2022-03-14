using AllOrNothing.Repository.Repositories;
using System;

namespace AllOrNothing.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository Players { get; }
        IQuestionSerieRepository QuestionSeries { get; }
        int Complete();
    }
}
