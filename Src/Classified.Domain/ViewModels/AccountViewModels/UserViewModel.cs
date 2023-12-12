using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Classified.Domain.Entities;

namespace Classified.Domain.ViewModels.AccountViewModels
{
    /// <summary>
    /// View Model for user Registeration
    /// </summary>
    public class UserViewModel
    {

    }

    public class UserProfileModelView
    {
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Role Name
        /// </summary>
        public string RoleName { get; set; }
    }

    /// <summary>
    /// View Model for user Registeration
    /// </summary>
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$&*])(?=.*[0-9]).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at least one upper case (A-Z), one lower case (a-z), one number (0-9) and special character (e.g. !@#$%^&*)")]

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage =
            "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// First Name of the User
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the User
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

    }

    /// <summary>
    /// View Model for user Registeration
    /// </summary>
    public class UserManageViewModel
    {
        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// First Name of the User
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the User
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [DisplayName("Comment")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "You comment must have between 10 to 200 charachters.", MinimumLength = 10)]
        public string Comment { get; set; }

    }

    /// <summary>
    /// View Model for user Registeration
    /// </summary>
    public class AdminUserRegisterViewModel
    {

        /// <summary>
        /// User Id
        /// </summary>
        public string Id { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$&*])(?=.*[0-9]).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at least one upper case (A-Z), one lower case (a-z), one number (0-9) and one special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        //[RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$&*])(?=.*[0-9]).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at least one upper case (A-Z), one lower case (a-z), one number (0-9) and special character (e.g. !@#$%^&*)")]

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage =
            "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// First Name of the User
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the User
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string LastName { get; set; }

        [DisplayName("Comment")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "You comment must have between 10 to 200 charachters.", MinimumLength = 10)]
        public string Comment { get; set; }

        /// <summary>
        /// Phone Number
        /// </summary>
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select user's Role")]
        public ApplicationRole Role { get; set; }

        /// <summary>
        /// List of Roles
        /// </summary>
        public IEnumerable<ApplicationRole> Roles { get; set; }

    }

    /// <summary>
    /// View Model for user Registeration
    /// </summary>
    public class AdminUserEditViewModel
    {

        /// <summary>
        /// User Id
        /// </summary>
        public string Id { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// First Name of the User
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the User
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string LastName { get; set; }

        [DisplayName("Comment")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "You comment must have between 10 to 200 charachters.", MinimumLength = 10)]
        public string Comment { get; set; }

        /// <summary>
        /// Phone Number
        /// </summary>
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select user's Role")]
        public ApplicationRole Role { get; set; }

        /// <summary>
        /// List of Roles
        /// </summary>
        public IEnumerable<ApplicationRole> Roles { get; set; }

    }

    /// <summary>
    /// View Model for user Registeration
    /// </summary>
    public class AdminProfileEditViewModel
    {

        /// <summary>
        /// User Id
        /// </summary>
        public string Id { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// First Name of the User
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the User
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string LastName { get; set; }

        [DisplayName("Comment")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "You comment must have between 10 to 200 charachters.", MinimumLength = 10)]
        public string Comment { get; set; }

        /// <summary>
        /// Phone Number
        /// </summary>
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User Role Name
        /// </summary>
        public string RoleName { get; set; }

    }

    /// <summary>
    /// View Model for the List of users in Admin Dashboard
    /// </summary>
    public class AdminUserManagmetViewModel
    {
        /// <summary>
        /// List of the exisiting roles
        /// </summary>
        public IEnumerable<ApplicationRole> Roles { get; set; }

        /// <summary>
        /// Role Name
        /// </summary>
        [Display(Name = "Role")]
        public string RoleName;

        /// <summary>
        /// List of the Users
        /// </summary>
        public IEnumerable<UserRegisterViewModel> Users { get; set; }



    }

    /// <summary>
    /// Use While user want to reset his/her password
    /// </summary>
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$&*])(?=.*[0-9]).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at least one upper case (A-Z), one lower case (a-z), one number (0-9) and one special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    /// <summary>
    /// Reset User Password View Model
    /// </summary>
    public class ResetUserPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$&*])(?=.*[0-9]).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at least one upper case (A-Z), one lower case (a-z), one number (0-9) and one special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// user while user ask for forget his password
    /// </summary>
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
