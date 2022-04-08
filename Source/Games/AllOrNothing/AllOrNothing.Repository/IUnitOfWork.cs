using AllOrNothing.Repository.Contracts;
using System;

namespace AllOrNothing.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository Players { get; }
        IQuestionSerieRepository QuestionSeries { get; }
        ITopicRepository Topics { get; }
        IQuestionRepository Questions { get; }
        ICompetenceRepository Competences { get; }
        string GetRepositoryNameForType(string typeName);
        int Complete();
    }
}
