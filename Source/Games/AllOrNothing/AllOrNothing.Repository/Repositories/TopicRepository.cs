using AllOrNothing.Data;
using AllOrNothing.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AllOrNothing.Repository.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(DbContext context) : base(context)
        {

        }
    }
}
