using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{

    public class QueuedEmailRepositories : RepositoryBase<QueuedEmail>, IQueuedEmailRepositories
    {

        public QueuedEmailRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IQueuedEmailRepositories : IRepository<QueuedEmail>
    {


    }
}
