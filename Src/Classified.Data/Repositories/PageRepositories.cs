using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{
    public class PageRepositories : RepositoryBase<Page>, IPageRepositories
    {
        public PageRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IPageRepositories : IRepository<Page>
    {


    }
}
