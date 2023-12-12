using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;

namespace Classified.Data.Repositories
{
    public class TopViewRepositories : RepositoryBase<TopView>, ITopViewRepositories
    {
        public TopViewRepositories(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public List<TopView> GetTopView()
        {

            List<TopView> data = DataContext.TopViews.SqlQuery("GetTopViewCount").ToList();
            return data;



        }
    }
    public interface ITopViewRepositories : IRepository<TopView>
    {

        List<TopView> GetTopView();
    }
}
