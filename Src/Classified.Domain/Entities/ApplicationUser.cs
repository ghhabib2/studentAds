
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Classified.Domain.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// First Name of the User
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the User
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Comment About the user
        /// </summary>
        [StringLength(200)]
        public string Comment { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Description of Added Role
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

    }
}
