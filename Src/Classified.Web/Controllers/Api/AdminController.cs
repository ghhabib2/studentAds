using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classified.Domain.Entities;
using Classified.Domain.Entities.Dtos.AdimDtos;
using Classified.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Classified.Data.Advertisements.Categories;


namespace Classified.Web.Controllers.Api
{
    public class AdminController : ApiController
    {

        //Define the Connection to the database
        private ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.Admin)]
        public string TestApi()
        {
            return "Hello World!!";
        }

        /// <summary>
        /// Delete the user selected
        /// </summary>
        /// <param name="email">Email Address</param>
        [HttpDelete]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<HttpResponseMessage> DeleteUser(string email)
        {
            
            //Create a sample user manager
            //Create the necessary tools for user management
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            //Fetch the user information
            var tempUser = await userManager.FindByEmailAsync(email);

            var result = await userManager.DeleteAsync(tempUser);
            if (result.Succeeded)
            {
                var messeage = Request.CreateResponse(result);
                return messeage;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }

        }

    }

    
}
