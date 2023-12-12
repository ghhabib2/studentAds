using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities.Dtos.AdimDtos
{
    /// <summary>
    /// Class for API public view Model
    /// </summary>
    public class AdminUserDtos
    {
        /// <summary>
        /// User's First Name
        /// </summary>
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Users Last Name
        /// </summary>
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// User's Email Address
        /// </summary>
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }



    }
}
