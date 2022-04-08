using AllOrNothing.Data;
using AllOrNothing.Repository.Contracts;
using AllOrNothing.Repository.Repositories;

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
            Topics = new TopicRepository(context);
            Questions = new QuestionRepository(context);
            Competences = new CompetenceRepository(context);
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

        public ITopicRepository Topics
        {
            get;
            private set;
        }

        public IQuestionRepository Questions
        {
            get;
            private set;
        }

        public ICompetenceRepository Competences
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

        public string GetRepositoryNameForType(string typeName)
        {
            switch (typeName)
            {
                case nameof(Player):
                    return nameof(Players);
                    break;
                case nameof(QuestionSerie):
                    return nameof(QuestionSeries);
                    break;
                case nameof(Topic):
                    return nameof(Topics);
                    break;
                case nameof(Question):
                    return nameof(Questions);
                    break;
                case nameof(Competence):
                    return nameof(Competences);
                    break;
                default:
                    return string.Empty;
                    break;
            }
        }
    }
}
