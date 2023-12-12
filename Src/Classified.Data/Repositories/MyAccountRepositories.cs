using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{

    public class MyAccountRepositories : RepositoryBase<MyAccount>, IMyAccountRepositories
    {

        public MyAccountRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IMyAccountRepositories : IRepository<MyAccount>
    {


    }
}
