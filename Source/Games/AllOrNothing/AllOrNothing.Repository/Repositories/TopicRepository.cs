using AllOrNothing.Data;
using AllOrNothing.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Repository.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository 
    {
        public TopicRepository(DbContext context) : base(context)
        {

        }
    }
}
