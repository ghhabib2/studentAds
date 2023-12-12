using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{
  
    public interface IOderDetailRepositories : IRepository<OrderDetail>
    {


    }

    public class OderDetailRepositories : RepositoryBase<OrderDetail>, IOderDetailRepositories
    {

        public OderDetailRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
