using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Classified.Data.Infrastructure;
using Classified.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Classified.Data.Repositories
{
    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void AssignRole(string Id, IEnumerable<string> roleNames)
        {

            foreach (string Role in roleNames)
            {
                await UserManager.AddToRoleAsync(Id, Role);
            }

            var user = this.GetById(id);
            if (user.Roles != null)
            {
                user.Roles.Clear();
            }
            else
            {
                |
                user.Roles=new List<Role>();
            }
            foreach (string roleName in roleNames)
            {
                var role = this.DataContext.Roles.Find(roleName);
                user.Roles.Add(role);
            }

            this.DataContext.Users.Attach(user);
            this.DataContext.Entry(user).State = EntityState.Modified;
            this.DataContext.SaveChanges();
        }

    }

    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void AssignRole(int id, List<string> roleName);
    }
}
