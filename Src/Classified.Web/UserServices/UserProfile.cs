using System.Linq;
using System.Web;
using Classified.Data;
using Classified.Domain.Entities;
using Classified.Domain.ViewModels.AccountViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Classified.Web.UserServices
{
    /// <summary>
    /// Returning the information of Current Online User
    /// </summary>
    public class UserProfile
    {
        public static UserProfileModelView UserProfileInfo
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //Define Temp Profile
                    var tempProfile = new UserProfileModelView();

                    // Check if the session exist
                    if (HttpContext.Current.Session["CurrentUserInfo"] == null)
                    {
                        //Open Connection
                        var _context = new ApplicationDbContext();

                        var user = _context.Users.SingleOrDefault(
                            c => c.Email == HttpContext.Current.User.Identity.Name);
                        
                        if (user != null)
                        {
                            tempProfile.FirstName = user.FirstName;
                            tempProfile.LastName = user.LastName;
                        }

                        var roleStore = new RoleStore<ApplicationRole>(_context);
                        var roleManager = new RoleManager<ApplicationRole>(roleStore);

                        var roleId = user.Roles.FirstOrDefault()?.RoleId;

                        tempProfile.RoleName =
                            roleManager.Roles.SingleOrDefault(c => c.Id == roleId)?.Name;

                        //Close Connection
                        _context.Dispose();

                        HttpContext.Current.Session.Add("CurrentUserInfo",tempProfile);
                    }

                    // Return Profile Model View of the Currrent User
                    return ((UserProfileModelView) HttpContext.Current.Session["CurrentUserInfo"]);
                }
                else
                {
                    //Try to remove this Session parameter
                    HttpContext.Current.Session.Remove("CurrentUserInfo");
                    return new UserProfileModelView();
                }
                
                
            }
        }
    }
}