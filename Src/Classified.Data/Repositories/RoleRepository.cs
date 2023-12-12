using Classified.Data.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Classified.Data.Repositories
{
    public class RoleRepository : RepositoryBase<IdentityRole>, IRoleRepository
    {
        public RoleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IRoleRepository : IRepository<IdentityRole>
    {

    }
}
