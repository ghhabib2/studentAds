using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{

    public interface IOrderTypeRepositories : IRepository<OrderType>
    {
       void GetAllList();
    }

    public class OrderTypeRepositories : RepositoryBase<OrderType>, IOrderTypeRepositories
    {
        public OrderTypeRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            
        }
        public  void GetAllList()
        {
            this.GetAll();
        }
    }
}
