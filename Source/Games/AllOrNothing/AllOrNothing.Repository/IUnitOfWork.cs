using AllOrNothing.Repository.Contracts;
using AllOrNothing.Repository.Repositories;
using System;

namespace AllOrNothing.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository Players { get; }
        IQuestionSerieRepository QuestionSeries { get; }
        ITopicRepository Topics { get; }
        IQuestionRepository Questions { get; }
        int Complete();
    }
}
