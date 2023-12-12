using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Classified.Data;
using Classified.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Classified.Web.Controllers
{
    [Authorize(Roles = RoleTypes.Admin)]
    public class AdminRoleController : Controller
    {
        // GET: AdminRole
        [Route("Account/RoleManager/")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Account/RoleManager/Create")]
        public async Task<ActionResult> Create()
        {
            //Temp Code
            var roleStore = new RoleStore<ApplicationRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            var tempRole = new ApplicationRole
            {
                Name = "AdvancedUser",
                Description = "The access level of these users is higher than the system's default user. They can approve the newly added advertisements."
            };


            await roleManager.CreateAsync(tempRole);
            

            return Content("OK");
        }

    }
}