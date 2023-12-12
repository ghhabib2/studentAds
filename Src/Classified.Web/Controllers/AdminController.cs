using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Classified.Data;
using Classified.Data.Advertisements.Advertisement;
using Classified.Data.Advertisements.Categories;
using Classified.Domain.Entities;
using Classified.Domain.ViewModels;
using Classified.Domain.ViewModels.AccountViewModels;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Services;
using Classified.Services.Advertisement;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Classified.Web.Controllers
{
    [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
    public class AdminController : Controller
    {

        #region Connection Creation and Dispose

        //Create a sample of Database Connection
        protected ApplicationDbContext Context;

        public AdminController()
        {
            //Open the connection to the database
            Context = new ApplicationDbContext();
        }

        /// <summary>
        /// Close Connection to the Database
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        /// <summary>
        /// Showing the Dashboard to the User
        /// </summary>
        /// <returns></returns>

        [Route("Admin/Index")]
        public ActionResult Index()
        {
            return View();
        }

        #region User Related Actions

        /// <summary>
        /// Open a View for the administrator to create a new user
        /// </summary>
        /// <returns>User Form View for registration of new user information</returns>
        [Authorize(Roles = RoleTypes.Admin)]
        [Route("Admin/UserManagment/New", Name = "InsertAdminUser")]
        public ActionResult NewUser()
        {
            //Fetch Role list from Database
            var roleStore = new RoleStore<ApplicationRole>(Context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            var tempRoles = roleManager.Roles.ToList();

            //Find the application Role where Role name is User
            var tempUserRole = tempRoles.SingleOrDefault(c => c.Name == RoleTypes.User);

            //Remove the user role from the list
            tempRoles.Remove(tempUserRole);

            //Create a sample of the administrator user register vie model for sending to view
            var tempViewModel = new AdminUserRegisterViewModel
            {

                Roles = tempRoles,
                FirstName = string.Empty,
                LastName = string.Empty,
                Comment = string.Empty,
                Email = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                PhoneNumber = string.Empty,
                Id = string.Empty
            };

            //send information to Views
            return View("UserForm", tempViewModel);
        }


        /// <summary>
        /// Open a View for the administrator to create a new user
        /// </summary>
        /// <returns>User Form View for registration of new user information</returns>
        [Authorize(Roles = RoleTypes.Admin)]
        [Route("Admin/UserManagment/Update", Name = "UpdateAdminUser")]
        public ActionResult EditUser(string email)
        {
            //Fetch the current user information from database
            var tempUser = Context.Users.Include(tb => tb.Roles).SingleOrDefault(c => c.Email == email);

            if (tempUser == null)
            {
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    "There is no user in the system with the information you provided.",
                    MessageStatus.Warning);
                
                //Redirect to Main Dashboard of Admins
                return RedirectToAction("Index", "Admin");
            }


            //Fetch Role list from Database
            var roleStore = new RoleStore<ApplicationRole>(Context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            var tempRoleId = tempUser.Roles.FirstOrDefault()?.RoleId;

            //Create a sample of the administrator user register model for sending to view
            var tempViewModel = new AdminUserEditViewModel
            {
                Roles = roleManager.Roles.ToList(),
                FirstName = tempUser.FirstName,
                LastName = tempUser.LastName,
                Comment = tempUser.Comment,
                Email = tempUser.Email,
                PhoneNumber = tempUser.PhoneNumber,
                Id = tempUser.Id,
                Role = roleManager.Roles.SingleOrDefault(c => c.Id == tempRoleId)
            };

            //send information to Views
            return View("UserEditForm", tempViewModel);
        }

        /// <summary>
        /// Create a new user or Update existing user Database
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AdminUserRegisterViewModel user)
        {
            //Check to see if the form is Valid
            if (!ModelState.IsValid)
            {
                //Fetch Role list from Database
                var roleStore = new RoleStore<ApplicationRole>(Context);
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                var tempViewModel = new AdminUserRegisterViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Comment = user.Comment,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    Roles = roleManager.Roles.ToList()
                };
                //Return to the View to check the information
                return View("UserForm", tempViewModel);
            }

            //Create the necessary tools for user management
            var userManager = HttpContext.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            //Control to choose the kind of operation
            //Check the user Id to see if it is empty.
            //Save Operation
            var tempUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                EmailConfirmed = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Comment = user.Comment,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = true
            };


            var result = await userManager.CreateAsync(tempUser, user.Password);

            if (result.Succeeded)
            {

                //Add selected role for the user
                await userManager.AddToRoleAsync(tempUser.Id, user.Role.Name);

                //Create new user Operation is successfully and system must render the view for new information 
                //With success message.

                //Sent the kind of message system must show to the end user
                PopUpMessageGenerator.GenerateMessage("User Management System",
                    "New user added successfully.",
                    MessageStatus.Successfull);
                
                //Render the View
                return RedirectToRoute("InsertAdminUser");

            }
            else
            {

                //There is a problem in the form and system must Render the view with error message

                //Fetch the Role information
                var roleStore = new RoleStore<ApplicationRole>(Context);
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                //Populate the Roles for the View
                user.Roles = roleManager.Roles.ToList();

                //Sent the kind of message system must show to the end user
                PopUpMessageGenerator.GenerateMessage("User Management System",
                    "There is a problem in user registration.",
                    MessageStatus.Failed);

                return View("UserForm", user);
            }



        }


        /// <summary>
        ///  Update existing user Database
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AdminUserEditViewModel user)
        {
            //Check to see if the form is Valid
            if (!ModelState.IsValid)
            {
                //Fetch Role list from Database
                var roleStore = new RoleStore<ApplicationRole>(Context);
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                var tempRoles = roleManager.Roles.ToList();

                //Find the application Role where Role name is User
                var tempUserRole = tempRoles.SingleOrDefault(c => c.Name == RoleTypes.User);

                //Remove the user role from the list
                tempRoles.Remove(tempUserRole);

                var tempViewModel = new AdminUserRegisterViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Comment = user.Comment,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    Roles = tempRoles
                };
                //Return to the View to check the information
                return View("UserForm", tempViewModel);
            }

            //Create the necessary tools for user management
            var userManager = HttpContext.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            //Update Operation
            var tempUser = userManager.FindById(user.Id);

            if (tempUser != null)
            {
                tempUser.UserName = user.Email;
                tempUser.Email = user.Email;
                tempUser.EmailConfirmed = true;
                tempUser.FirstName = user.FirstName;
                tempUser.LastName = user.LastName;
                tempUser.Comment = user.Comment;
                tempUser.PhoneNumber = user.PhoneNumber;
                tempUser.PhoneNumberConfirmed = true;
            }

            var result = await userManager.UpdateAsync(tempUser);

            if (result.Succeeded)
            {

                //Update operation is successful. The system must show a message to the user
                //Fetch the Role information
                var roleStore = new RoleStore<ApplicationRole>(Context);
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                //Check user Role
                var userCurrentRole = roleManager.Roles.SingleOrDefault(c => c.Name == user.Role.Name);

                if (userCurrentRole != null && (tempUser != null && userCurrentRole.Id != tempUser.Roles.First().RoleId))
                {
                    //User Role Changes. Delete the previous Role for the user, and Add the new role
                    //Get the previous Role name
                    var tempPreviousRoleId = tempUser.Roles.FirstOrDefault()?.RoleId;

                    var previousRoleName = roleManager.Roles
                        .SingleOrDefault(c => c.Id == tempPreviousRoleId)
                        ?.Name;

                    //Remove user from previous Role
                    result = await userManager.RemoveFromRoleAsync(user.Id, previousRoleName);

                    if (result.Succeeded)
                    {
                        //Add new Role for the user
                        await userManager.AddToRoleAsync(user.Id, user.Role.Name);
                    }

                }

                //Sent the kind of message system must show to the end user
                PopUpMessageGenerator.GenerateMessage("User Management System",
                    "User information has been updated successfully.",
                    MessageStatus.Successfull);
                
                return RedirectToAction("UserManagement", "Admin");

            }
            else
            {

                //There is a problem in the form and system must Render the view with error message

                //Fetch the Role information
                var roleStore = new RoleStore<ApplicationRole>(Context);
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                //Populate the Roles for the View
                user.Roles = roleManager.Roles.ToList();

                //Sent the kind of message system must show to the end user
                PopUpMessageGenerator.GenerateMessage("User Management System",
                    "There is a problem in updating user information.",
                    MessageStatus.Failed);
                
                return View("UserEditForm", user);
            }

        }

        /// <summary>
        /// Open the user list by default value
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin)]
        public ActionResult FirstTimeRunUserManagement()
        {

            //Set the value of UserManagment Model View in order to send to the View
            //Fetch the Role information
            var roleStore = new RoleStore<ApplicationRole>(Context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            var tempRoles = roleManager.Roles.Include(tb => tb.Users).ToList();

            //Find the application Role where Role name is User
            var tempUserRole = tempRoles.SingleOrDefault(c => c.Name == RoleTypes.User);

            //Remove the user role from the list
            tempRoles.Remove(tempUserRole);

            //Return the target role Id
            var targetRoleId = tempRoles.FirstOrDefault()?.Id;

            // Create a templae user
            List<UserRegisterViewModel> tempUsers;

            //Check if there is a role to user record
            if (!string.IsNullOrEmpty(targetRoleId))
            {
                //Filter the list of users based on the records of role to user table
                tempUsers = Context.Users.Include(m => m.Roles)
                    .Where(user => user.Roles.Select(role => role.RoleId).Contains(targetRoleId))
                    .Select(user =>
                       new UserRegisterViewModel
                       {
                           FirstName = user.FirstName,
                           LastName = user.LastName,
                           Email = user.Email
                       }).ToList();
            }
            else
            {
                //  If there is no role exist, then system should send an empty list
                tempUsers = new List<UserRegisterViewModel>();
            }

            //Create the Moel View
            var tempModelView = new AdminUserManagmetViewModel
            {
                Roles = tempRoles,
                Users = tempUsers
            };

            // Return the model View  to usermanagment View
            return View("UserManagement", tempModelView);
        }

        /// <summary>
        /// Return the specific Role Users to View
        /// </summary>
        /// <param name="roleName">Role Name</param>
        /// <returns>Return a UserRegisterViewModel to View</returns>
        [Authorize(Roles = RoleTypes.Admin)]
        //[Route("Admin/UserManagment}", Name = "UserManagmentIndexWithParma")]
        public ActionResult UserManagement(string roleName)
        {

            if (string.IsNullOrEmpty(roleName))
            {
                return FirstTimeRunUserManagement();
            }

            //Set the value of UserManagment Model View in order to send to the View
            //Fetch the Role information
            var roleStore = new RoleStore<ApplicationRole>(Context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            //Tech the list of the system Roles
            var tempRoles = roleManager.Roles.Include(tb => tb.Users).ToList();

            //Find the application Role where Role name is User
            var tempUserRole = tempRoles.SingleOrDefault(c => c.Name == RoleTypes.User);

            //Remove the user role from the list
            tempRoles.Remove(tempUserRole);

            //Filter the information of roles list based on the parameter sent to the form
            //Return the target role Id
            var targetRoleId = tempRoles.SingleOrDefault(c => c.Name == roleName)?.Id;

            //Create a template user for Model View usage
            List<UserRegisterViewModel> tempUsers;

            //Check if there is a role to user record
            if (!string.IsNullOrEmpty(targetRoleId))
            {

                //Filter the list of users based on the records of role to user table
                tempUsers = Context.Users.Include(m => m.Roles)
                    .Where(user => user.Roles.Select(role => role.RoleId).Contains(targetRoleId))
                    .Select(user =>
                        new UserRegisterViewModel
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email
                        }).ToList();
            }
            else
            {
                //  If there is no role exist, then system should send an empty list
                tempUsers = new List<UserRegisterViewModel>();
            }

            //Create the Model View
            var tempModelView = new AdminUserManagmetViewModel
            {
                Roles = tempRoles,
                RoleName = roleName,
                Users = tempUsers
            };
            // Return the model View  to usermanagment View
            return View("UserManagement", tempModelView);
        }


        /// <summary>
        /// Populate the Profile Management form at the first time
        /// </summary>
        /// <returns>View of Profile Management</returns>
        public ActionResult ProfileManagement()
        {
            //Fill the form before sending to the View
            //Create a sample of User Manager
            //Create the necessary tools for user management
            var userManager = HttpContext.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            //Fetch the information of the user
            var tempUser = userManager.FindByEmail(User.Identity.GetUserName());

            //Check if the user is available 
            if (tempUser == null)
            {
                //Show the user that there is a problem on fetching his/her information
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    "There is a problem on retrieving user information.",
                    MessageStatus.Failed);
                
                //Return to the Index page of Admin dashboard
                return RedirectToAction("Index", "Admin");
            }

            //Fetch the Role information
            var roleStore = new RoleStore<ApplicationRole>(Context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            //Fetch the Role Id
            var tempRoleId = tempUser.Roles.FirstOrDefault()?.RoleId;

            //Fetch the Role information
            var userRole = roleManager.Roles.SingleOrDefault(c => c.Id == tempRoleId);

            //Populate the model
            var tempModle = new AdminProfileEditViewModel
            {
                FirstName = tempUser.FirstName,
                LastName = tempUser.LastName,
                Email = tempUser.Email,
                PhoneNumber = tempUser.PhoneNumber,
                Comment = tempUser.Comment,
                RoleName = userRole?.Name,
                Id = tempUser.Id,
            };

            //Return the View
            return View(tempModle);
        }

        /// <summary>
        /// Edit the user information after submitting the form
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProfileManagement(AdminProfileEditViewModel user)
        {
            //Check to see if the form is Valid
            if (!ModelState.IsValid)
            {
                var tempViewModel = new AdminProfileEditViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Comment = user.Comment,
                    PhoneNumber = user.PhoneNumber,
                    RoleName = user.RoleName
                };
                //Return to the View to check the information
                return View("ProfileManagement", tempViewModel);
            }

            //Create the necessary tools for user management
            var userManager = HttpContext.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            //Update Operation
            var tempUser = userManager.FindById(user.Id);

            if (tempUser != null)
            {
                tempUser.UserName = user.Email;
                tempUser.Email = user.Email;
                tempUser.EmailConfirmed = true;
                tempUser.FirstName = user.FirstName;
                tempUser.LastName = user.LastName;
                tempUser.Comment = user.Comment;
                tempUser.PhoneNumber = user.PhoneNumber;
                tempUser.PhoneNumberConfirmed = true;
            }

            var result = await userManager.UpdateAsync(tempUser);

            if (result.Succeeded)
            {

                //Sent the kind of message system must show to the end user
                PopUpMessageGenerator.GenerateMessage("User Management System",
                    "User profile information has been updated successfully.",
                    MessageStatus.Successfull);
                
                return RedirectToAction("Index", "Admin");

            }
            else
            {

                //Sent the kind of message system must show to the end user
                PopUpMessageGenerator.GenerateMessage("User Management System",
                    "There is a problem in updating user information.",
                    MessageStatus.Failed);

                return View("ProfileManagement", user);
            }
        }

        /// <summary>
        /// Show the user Password Form
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult ResetUserPassword(string email)
        {
            //Fetch User Information

            //Create a sample of User Manager
            var userManager = HttpContext.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            var tempUser = userManager.FindByEmail(email);

            var tempModel = new ResetUserPasswordViewModel
            {
                Email = email,
                FirstName = tempUser.FirstName,
                LastName = tempUser.LastName
            };

            return View(tempModel);
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeUserPassword(ResetUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetUserPassword", model);
            }

            //Create a sample of User Manager
            //Create the necessary tools for user management
            var userManager = HttpContext.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            //Fetch the user information based on his/her email address
            var user = await userManager.FindByNameAsync(model.Email);



            //Check if the user exist
            if (user == null)
            {
                //Show the user that there is a problem on fetching his/her information
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    "There is a problem on retrieving user information.",
                    MessageStatus.Failed);
                
                return RedirectToAction("UserManagement", "Admin");
            }


            try
            {
                await userManager.RemovePasswordAsync(user.Id);
                await userManager.AddPasswordAsync(user.Id, model.Password);

                //Show the user that his or her password has been changed successfully.
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    $"{model.FirstName} {model.LastName} password has been updated successfully.",
                    MessageStatus.Successfull);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
                //Show the user that there is a problem on fetching his/her information
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    "There is an internal server error while we tried to update your password. Please contact system administration",
                    MessageStatus.Failed);

            }

            return RedirectToAction("UserManagement", "Admin");
        }

        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult ResetPassword()
        {

            //Create a sample of View Model and send it to the form
            var tempModel = new ResetPasswordViewModel
            {
                Email = User.Identity.GetUserName()
            };

            return View(tempModel);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Create a sample of User Manager
            //Create the necessary tools for user management
            var userManager = HttpContext.GetOwinContext().GetUserManager<Services.User.ApplicationUserManager>();

            //Fetch the user information based on his/her email address
            var user = await userManager.FindByNameAsync(model.Email);

            //Check if the user exist
            if (user == null)
            {
                //Show the user that there is a problem on fetching his/her information
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    "There is a problem on retrieving user information.",
                    MessageStatus.Failed);

                return RedirectToAction("Index", "Admin");
            }



            try
            {
                await userManager.RemovePasswordAsync(user.Id);
                await userManager.AddPasswordAsync(user.Id, model.Password);

                //Show the user that his or her password has been changed successfully.
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    "Your password has been updated successfully.",
                    MessageStatus.Successfull);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
                //Show the user that there is a problem on fetching his/her information
                PopUpMessageGenerator.GenerateMessage("User Registration System",
                    "There is an internal server error while we tried to update your password. Please contact system admisnitration",
                    MessageStatus.Failed);

            }

            return RedirectToAction("Index", "Admin");
        }

        #endregion

        #region Advertisement Related Actions


        #region Categories Related Methods

        /// <summary>
        /// Open a List of Existing Categories for the user
        /// </summary>
        /// <returns>The Main List of Categories for the User</returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult NewCategoryManagement()
        {
            //Create a temp Category List
            var tempCategoriesList = new CategoriesCore().GetAll().OrderBy(item => item.Name).ToList();

            // List for ComboBox
            var tempListComboForDropDown = HirarchyList(tempCategoriesList);
            // Select the root Categories
            var tempListCategroiesTable = tempCategoriesList.Where(c => c.ParentCategory is null).ToList();

            //Create the temp View Model to send to Send
            var tempModel = new CategoryManagmentViewMoel
            {
                CategoryId = -1,
                HierarchyCategoryList = tempListComboForDropDown,
                CategoryType = 0,
                CategoryTypes = PopulateCategoryTypes(),
                TargetCategories = tempListCategroiesTable
            };

            // Return Target View
            return View("CategoryManagement", tempModel);
        }

        /// <summary>
        /// Open a List of Existing Categories for the user
        /// </summary>
        /// <returns>The Main List of Categories for the User</returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult CategoryManagement(CategoryManagmentViewMoel model)
        {

            if (model.CategoryId == -1 && model.CategoryType == 0)
            {
                return NewCategoryManagement();
            }

            //Retrieve the list of all categories
            var tempCategoriesList = new CategoriesCore().GetAll().OrderBy(item => item.Name).ToList();

            // Create a temp List for Category Table 
            List<CategoryViewMoel> tempListCategroiesTable;


            // Select the root Categories
            switch (model.CategoryType)
            {
                case 1:
                    {
                        tempListCategroiesTable = model.CategoryId == -1 ? tempCategoriesList.Where(c => c.Published == true && c.ParentCategoryId is null).ToList() : tempCategoriesList.Where(c => c.Published == true && c.ParentCategoryId == model.CategoryId).ToList();
                        break;
                    }
                case 2:
                    {
                        tempListCategroiesTable = model.CategoryId == -1 ? tempCategoriesList.Where(c => c.Deleted == true && c.ParentCategoryId is null).ToList() : tempCategoriesList.Where(c => c.Deleted == true && c.ParentCategoryId == model.CategoryId).ToList();
                        break;
                    }
                default:
                    {
                        tempListCategroiesTable = model.CategoryId == -1 ? tempCategoriesList.Where(c => c.ParentCategoryId is null).ToList() : tempCategoriesList.Where(c => c.ParentCategoryId == model.CategoryId).ToList();
                        break;
                    }
            }


            //Create a temp Category List
            //=================================================================

            // List for ComboBox
            var tempListComboForDropDown = HirarchyList(tempCategoriesList);


            //Create the temp View Model to send to Send
            var tempModel = new CategoryManagmentViewMoel
            {
                CategoryId = model.CategoryId,
                HierarchyCategoryList = tempListComboForDropDown,
                CategoryType = model.CategoryType,
                CategoryTypes = PopulateCategoryTypes(),
                TargetCategories = tempListCategroiesTable
            };

            // Return Target View
            return View(tempModel);

        }

        /// <summary>
        /// Create a New Category
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [Route("Admin/CategoryManagement/{id}", Name = "categoryItem")]
        public ActionResult Category(int Id)
        {


            if (Id == -1)
            {
                //This is a new Record
                var tempModel = new CategoryRegisterViewMoel
                {
                    Category = new CategoryViewMoel(),
                    CateogryList = HirarchyList(new CategoriesCore().GetAll().OrderBy(item => item.Name).ToList()),
                    ParentCategoryId = -1
                };

                return View("CategoryForm", tempModel);
            }
            else
            {
                //select the Category from database and display its information in Form
                var tempCategory = new CategoriesCore().Get(item => item.Id == Id);

                if (tempCategory != null)
                {
                    //This is a new Record
                    var tempModel = new CategoryRegisterViewMoel
                    {
                        Category = tempCategory,
                        CateogryList = HirarchyList(new CategoriesCore().GetAll().OrderBy(item => item.Name).ToList()),
                        ParentCategoryId = tempCategory.ParentCategoryId ?? -1
                    };
                    return View("CategoryForm", tempModel);
                }
                else
                {
                    //Show Error Message
                    //Show the user that his or her password has been changed successfully.
                    PopUpMessageGenerator.GenerateMessage("Category Management System",
                        "There is no category with provided information in the database.",
                        MessageStatus.Warning);

                    //Redirect to Category Management Page
                    return RedirectToAction("CategoryManagement");
                }

            }

        }


        /// <summary>
        /// Insert or update the Categories information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [ValidateAntiForgeryToken]
        public ActionResult CategorySave(CategoryRegisterViewMoel model)
        {
            if (!ModelState.IsValid)
            {
                //Create new model template
                model.CateogryList = HirarchyList(new CategoriesCore().GetAll().OrderBy(item => item.Name).ToList());

                return View("CategoryForm", model);
            }

            //Check if the model contains the information of a new model?
            if (model.Category.Id == -1)
            {
                //This is a new Model
                //Populate the parent category Id based on model information

                model.Category.ParentCategoryId = (model.ParentCategoryId == -1 ? (int?)null : model.ParentCategoryId);

                //Set the model Creation and update date
                model.Category.CreatedOnUtc = DateTime.UtcNow;
                model.Category.UpdatedOnUtc = DateTime.UtcNow;

                if (new CategoriesCore().Insert(model.Category))
                {
                    //Return Success Message
                    PopUpMessageGenerator.GenerateMessage("Category Management System",
                        "The new category has been added successfully.",
                        MessageStatus.Successfull);
                    
                    //Redirect to Category Management Page
                    return RedirectToAction("CategoryManagement");
                }
                else
                {
                    //Return Failed Message
                    PopUpMessageGenerator.GenerateMessage("Category Management System",
                        "There is a problem on registering new category information.",
                        MessageStatus.Failed);
                    
                    //Redirect to Category Management Page
                    return RedirectToAction("CategoryManagement");
                }
            }
            else
            {
                //This is a update Model    
                //Populate the parent category Id based on model information
                model.Category.ParentCategoryId = (model.ParentCategoryId == -1 ? (int?)null : model.ParentCategoryId);

                //Set the model update date
                model.Category.UpdatedOnUtc = DateTime.UtcNow;

                if (new CategoriesCore().Update(model.Category, m => m.Id == model.Category.Id))
                {
                    //Return Success Message
                    PopUpMessageGenerator.GenerateMessage("Category Management System",
                        "Selected category has been updated successfully.",
                        MessageStatus.Successfull);
                    
                    //Redirect to Category Management Page
                    return RedirectToAction("CategoryManagement");
                }
                else
                {
                    //Return Failed Message
                    PopUpMessageGenerator.GenerateMessage("Category Management System",
                        "There is a problem on updating category information.",
                        MessageStatus.Failed);
                    
                    //Redirect to Category Management Page
                    return RedirectToAction("CategoryManagement");
                }

            }
        }

        /// <summary>
        /// Delete the Selected Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To the category management page</returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [Route("Admin/DeleteCategory/{id}", Name = "DeleteCategory")]
        public ActionResult DeleteCategory(int id)
        {
            //Delete the record and return the result
            if (new CategoriesCore().Delete(item => item.Id == id))
            {
                //The Delete operation is successful show the success message
                //Show Error Message
                //Show the user that his or her password has been changed successfully.
                //Return Failed Message
                PopUpMessageGenerator.GenerateMessage("Category Management System",
                    "Selected Category Deleted Successfully.",
                    MessageStatus.Successfull);

                return RedirectToAction("CategoryManagement");
            }
            else
            {
                //The delete operation is not successful. Sow the failed message
                //Show Error Message
                //Show the user that his or her password has been changed successfully.
                PopUpMessageGenerator.GenerateMessage("Category Management System",
                    "There is a problem in deleting the selected category.",
                    MessageStatus.Failed);
                
                return RedirectToAction("CategoryManagement");
            }
        }

        /// <summary>
        /// Populate the Categories type
        /// </summary>
        /// <returns>Return a list of all categories</returns>
        protected internal IEnumerable<CategoryType> PopulateCategoryTypes()
        {
            var templist = new List<CategoryType>();

            templist.Add(new CategoryType { Id = 0, Name = "All" });
            templist.Add(new CategoryType { Id = 1, Name = "Published" });
            templist.Add(new CategoryType { Id = 2, Name = "Deleted" });

            return templist;
        }

        /// <summary>
        /// Get the List of Categories and convert it to Presentable Hierarchy List for View Purpose
        /// </summary>
        /// <param name="basicList"></param>
        /// <returns></returns>
        protected internal IEnumerable<CategoryItemViewMoel> HirarchyList(List<CategoryViewMoel> basicList)
        {


            //Select the Parent Nodes.
            var categoryViewMoels = basicList.ToList();
            var parentNodes = categoryViewMoels.Where(c => c.ParentCategory == null).ToList();

            var outputList = new List<CategoryItemViewMoel>();

            //Add the Root to the outputList
            outputList.Add(new CategoryItemViewMoel
            {
                Name = "Root",
                Id = -1
            });

            foreach (var category in parentNodes)
            {
                //Add the Item to the List
                outputList.Add(new CategoryItemViewMoel
                {
                    Name = $"-- {category.Name}",
                    Id = category.Id
                });



                //Check for children nodes
                if (categoryViewMoels.Count(c => c.ParentCategoryId == category.Id) > 0)
                {
                    //Add the children to the list
                    AppendChildren(ref outputList, category, ref basicList, "----");
                }
            }

            return outputList;
        }

        /// <summary>
        /// Add the children to the output List 
        /// </summary>
        /// <param name="outputList">The list that should be generated as Output list</param>
        /// <param name="category">Target Category to add its child</param>
        /// <param name="basicList">Basic List generated for the purpose of processing</param>
        /// <param name="paddingLevel">Padding level that should be added to the</param>
        protected internal void AppendChildren(ref List<CategoryItemViewMoel> outputList, CategoryViewMoel category,
            ref List<CategoryViewMoel> basicList, string paddingLevel)
        {
            var childList = basicList.Where(c => c.ParentCategoryId == category.Id).ToList();

            foreach (var categoryItem in childList)
            {
                outputList.Add(new CategoryItemViewMoel
                {
                    Id = categoryItem.Id,
                    Name = $"{paddingLevel} {categoryItem.Name}"
                });

                if (basicList.Count(c => c.ParentCategoryId == categoryItem.Id) > 0)
                {
                    AppendChildren(ref outputList, categoryItem, ref basicList, "----" + paddingLevel);
                }
            }

        }

        #endregion

        #region Categories Attributes Related Methods and Actions

        /// <summary>
        /// Action for displaying the View of Attribute Management Form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult CategoryAttributesManager(CategoryAttributesManagmentViewModel model)
        {

            //Return the View
            return View(PopulateCategoryAttributesViewModelInsert(model));
        }


        /// <summary>
        /// Insert or update the Categories Attribute information
        /// </summary>
        /// <param name="model">model as CategoryAttributesManagmentViewModel</param>
        /// <returns>To the Attribute Management Action</returns>
        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCategoryAttribute(CategoryAttributesManagmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CategoryAttributesManager", PopulateCategoryAttributesViewModelUpdate(model));
            }

            //Check if the model contains the information of a new model?
            if (model.CategoryAttributeId == -1)
            {
                //This is a new Model

                model.CategoryAttribut.ClassifiedCategoryId = model.CategoryId;


                if (new CategoryAttributesCore().Insert(model.CategoryAttribut))
                {
                    //Return Success Message
                    PopUpMessageGenerator.GenerateMessage("Category Attributes Management System",
                        "The new attribute has been added successfully.",
                        MessageStatus.Successfull);
                    
                    //Prepare from for new entry
                    var tempModel = new CategoryAttributesManagmentViewModel
                    {
                        CategoryAttributeId = -1,
                        CategoryId = model.CategoryId
                    };

                    //Redirect to Attribute Management Page
                    return RedirectToAction("CategoryAttributesManager", PopulateCategoryAttributesViewModelInsert(tempModel));
                }
                else
                {

                    //Return Failed Message
                    PopUpMessageGenerator.GenerateMessage("Category Attributes Management System",
                        "There is a problem on registering new attribute information.",
                        MessageStatus.Failed);

                    //Redirect to Attribute Management Page
                    return View("CategoryAttributesManager", PopulateCategoryAttributesViewModelUpdate(model));
                }
            }
            else
            {
                //This is a update Model    
                model.CategoryAttribut.ClassifiedCategoryId = model.CategoryId;
                model.CategoryAttribut.Id = model.CategoryAttributeId;

                if (new CategoryAttributesCore().Update(model.CategoryAttribut, m => m.Id == model.CategoryAttributeId))
                {
                    //Return Success Message
                    PopUpMessageGenerator.GenerateMessage("Category Attributes Management System",
                        "Selected attribute has been updated successfully.",
                        MessageStatus.Successfull);

                    //Prepare from for new entry
                    var tempModel = new CategoryAttributesManagmentViewModel
                    {
                        CategoryAttributeId = -1,
                        CategoryId = model.CategoryId
                    };

                    //Redirect to Attribute Management Page
                    return RedirectToAction("CategoryAttributesManager", PopulateCategoryAttributesViewModelInsert(tempModel));

                }
                else
                {
                    //Return Failed Message
                    PopUpMessageGenerator.GenerateMessage("Category Attributes Management System",
                        "There is a problem on updating attribute information.",
                        MessageStatus.Failed);

                    //Redirect to Attribute Management Page
                    return View("CategoryAttributesManager", PopulateCategoryAttributesViewModelUpdate(model));
                }

            }
        }

        /// <summary>
        /// Populate the Category view Model in order to send it to the View 
        /// </summary>
        /// <param name="model">View Model CategoryAttributesManagmentViewModel</param>
        /// <returns>View Model As CategoryAttributesManagmentViewModel</returns>
        protected internal CategoryAttributesManagmentViewModel PopulateCategoryAttributesViewModelInsert(
            CategoryAttributesManagmentViewModel model)
        {

            //Create a Sample View Model in order to send to View
            var tempCategoryAttribute = new CategoryAttributesViewModel();

            //Remove the root from the list


            if (model.CategoryAttributeId != -1)
            {
                //Load data from database
                tempCategoryAttribute = new CategoryAttributesCore().Get(item => item.Id == model.CategoryAttributeId);
            }
            else
            {
                //Load data from database
                if (model.CategoryAttribut != null && (model.CategoryAttribut != null || !string.IsNullOrEmpty(model.CategoryAttribut.AttributeName)))
                {
                    tempCategoryAttribute = model.CategoryAttribut;
                }
            }

            //Create a List for Categories that are published and Active
            var tempCategoryList = HirarchyList(new CategoriesCore()
                .GetMany(item => item.IsActive &&
                                 item.Published)
                .OrderBy(item => item.Name).ToList()).ToList();

            //Remove the Root from Category Lits
            tempCategoryList.RemoveAll(item => item.Id == -1);


            if (model.CategoryId == -1)
            {
                model.CategoryId = (tempCategoryList.FirstOrDefault() != null ? tempCategoryList.FirstOrDefault().Id : -1);
            }

            //Populate the Attribute List based on the information of selected category.
            var tempAttributeList = new CategoryAttributesCore().GetMany(item => item.ClassifiedCategoryId == model.CategoryId);

            var tempModel = new CategoryAttributesManagmentViewModel
            {
                HierarchyCategoryList = tempCategoryList,
                CategoryId = model.CategoryId,
                CategoryAttribut = tempCategoryAttribute,
                CategoryAttributeId = model.CategoryAttributeId,
                CategoryAttributes = tempAttributeList
            };

            //Return the finally generated View Model
            return tempModel;
        }

        /// <summary>
        /// Populate the Category view Model in order to send it to the View 
        /// </summary>
        /// <param name="model">View Model CategoryAttributesManagmentViewModel</param>
        /// <returns>View Model As CategoryAttributesManagmentViewModel</returns>
        protected internal CategoryAttributesManagmentViewModel PopulateCategoryAttributesViewModelUpdate(
            CategoryAttributesManagmentViewModel model)
        {
            //Create a List for Categories that are published and Active
            var tempCategoryList = HirarchyList(new CategoriesCore()
                .GetMany(item => item.IsActive &&
                                 item.Published)
                .OrderBy(item => item.Name).ToList()).ToList();

            //Remove the Root from Category Lits
            tempCategoryList.RemoveAll(item => item.Id == -1);

            //Populate the Attribute List based on the information of selected category.
            var tempAttributeList = new CategoryAttributesCore().GetMany(item => item.ClassifiedCategoryId == model.CategoryId);

            var tempModel = new CategoryAttributesManagmentViewModel
            {
                CategoryId = model.CategoryId,
                CategoryAttributeId = model.CategoryAttributeId,
                CategoryAttribut = model.CategoryAttribut,
                HierarchyCategoryList = tempCategoryList,
                CategoryAttributes = tempAttributeList
            };

            //Return the finally generated View Model
            return tempModel;
        }



        /// <summary>
        /// Delete the Selected Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To the category management page</returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [Route("Admin/DeleteCategoryAttribute/{id}/{catId}", Name = "DeleteCategoryAttribute")]
        public ActionResult DeleteCategoryAttribute(int id, int catId)
        {
            //Delete the record and return the result
            if (new CategoryAttributesCore().Delete(item => item.Id == id))
            {
                //The Delete operation is successful show the success message
                //Show Error Message
                //Show the user that his or her password has been changed successfully.
                PopUpMessageGenerator.GenerateMessage("Category Management System",
                    "Selected Category Deleted Successfully.",
                    MessageStatus.Successfull);


                //Prepare from for new entry
                var tempModel = new CategoryAttributesManagmentViewModel
                {
                    CategoryAttributeId = -1,
                    CategoryId = catId
                };

                //Redirect to Attribute Management Page
                return RedirectToAction("CategoryAttributesManager", PopulateCategoryAttributesViewModelInsert(tempModel));
            }
            else
            {
                //The delete operation is not successful. Sow the failed message
                //Show Error Message
                //Show the user that his or her password has been changed successfully.
                PopUpMessageGenerator.GenerateMessage("Category Management System",
                    "There is a problem in deleting the selected category.",
                    MessageStatus.Failed);

                //Prepare from for new entry
                var tempModel = new CategoryAttributesManagmentViewModel
                {
                    CategoryAttributeId = id,
                    CategoryId = catId
                };


                //Redirect to Attribute Management Page
                return View("CategoryAttributesManager", PopulateCategoryAttributesViewModelInsert(tempModel));

            }
        }

        #endregion

        #region Categories Attributes Values Related Methods and Actions

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult CategoryAttributeValuesManager(CategoryAttributeValuesManagementViewModelate model)
        {
            //Return the View
            return View(PopulateCategoryAttributeVluesViewModelInsert(model));
        }


        /// <summary>
        /// Insert or update the Categories Attribute information
        /// </summary>
        /// <param name="model">model as CategoryAttributesManagmentViewModel</param>
        /// <returns>To the Attribute Management Action</returns>
        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCategoryAttributeValue(CategoryAttributeValuesManagementViewModelate model)
        {
            //Check if the Model is Valid
            if (!ModelState.IsValid)
            {
                return View("CategoryAttributeValuesManager", PopulateCategoryAttributeVluesViewModelInsert(model));
            }

            //Check if the model contains the information of a new model?
            if (model.AttributeValueId == -1)
            {
                //This is a new Model

                model.AttributeValue.ClassifiedCategoryAttributeId = model.CategoryAttributeId;


                if (new CategoryAttributeValuesCore().Insert(model.AttributeValue))
                {
                    //Return Success Message
                    PopUpMessageGenerator.GenerateMessage("Category Attribute Values Management System",
                        "The new attribute value has been added successfully.",
                        MessageStatus.Successfull);
                    
                    //Prepare from for new entry
                    var tempModel = new CategoryAttributeValuesManagementViewModelate()
                    {
                        CategoryAttributeId = model.CategoryAttributeId,
                        CategoryId = model.CategoryId,
                        AttributeValueId =-1
                    };


                    //Redirect to Attribute Management Page
                    return RedirectToAction("CategoryAttributeValuesManager", PopulateCategoryAttributeVluesViewModelInsert(tempModel));
                }
                else
                {

                    //Return Failed Message
                    PopUpMessageGenerator.GenerateMessage("Category Attribute Values Management System",
                        "There is a problem on registering new attribute value information.",
                        MessageStatus.Failed);

                    //Prepare from for new entry
                    var tempModel = new CategoryAttributeValuesManagementViewModelate()
                    {
                        CategoryAttributeId = model.CategoryAttributeId,
                        CategoryId = model.CategoryId,
                        AttributeValueId = -1,
                        AttributeValue = model.AttributeValue
                    };

                    //Redirect to Attribute Management Page
                    return View("CategoryAttributeValuesManager", PopulateCategoryAttributeVluesViewModelInsert(model));
                }
            }
            else
            {
                //This is a update Model    
                model.AttributeValue.ClassifiedCategoryAttributeId = model.CategoryAttributeId;
                model.AttributeValue.Id = model.AttributeValueId;

                if (new CategoryAttributeValuesCore().Update(model.AttributeValue, m => m.Id == model.AttributeValueId))
                {
                    //Return Success Message
                    PopUpMessageGenerator.GenerateMessage("Category Attribute Values Management System",
                        "Selected attribute value has been updated successfully.",
                        MessageStatus.Successfull);

                    //Prepare from for new entry
                    var tempModel = new CategoryAttributeValuesManagementViewModelate()
                    {
                        CategoryAttributeId = model.CategoryAttributeId,
                        CategoryId = model.CategoryId,
                        AttributeValueId = -1
                    };


                    //Redirect to Attribute Management Page
                    return RedirectToAction("CategoryAttributeValuesManager", PopulateCategoryAttributeVluesViewModelInsert(tempModel));

                }
                else
                {
                    //Return Failed Message
                    PopUpMessageGenerator.GenerateMessage("Category Attribute Values Management System",
                        "There is a problem on updating attribute value information.",
                        MessageStatus.Failed);

                    //Prepare from for new entry
                    var tempModel = new CategoryAttributeValuesManagementViewModelate()
                    {
                        CategoryAttributeId = model.CategoryAttributeId,
                        CategoryId = model.CategoryId,
                        AttributeValueId = model.AttributeValueId,
                        AttributeValue = model.AttributeValue
                    };

                    //Redirect to Attribute Management Page
                    return View("CategoryAttributeValuesManager", PopulateCategoryAttributeVluesViewModelUpdates(tempModel));
                }

            }
        }


        /// <summary>
        /// Populate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal CategoryAttributeValuesManagementViewModelate PopulateCategoryAttributeVluesViewModelInsert(CategoryAttributeValuesManagementViewModelate model)
        {

            //Create a List for Categories that are published and Active
            var tempCategoryList = HirarchyList(new CategoriesCore()
                .GetMany(item => item.IsActive &&
                                 item.Published)
                .OrderBy(item => item.Name).ToList()).ToList();

            //Remove the Root from Category Lits
            tempCategoryList.RemoveAll(item => item.Id == -1);


            if (model.CategoryId == -1)
            {
                model.CategoryId = (tempCategoryList.FirstOrDefault() != null ? tempCategoryList.FirstOrDefault().Id : -1);
            }


            //Create a Sample View Model in order to send to View and populate it
            var tempCategoryAttributeList = new CategoryAttributesCore().GetMany(item => item.ClassifiedCategoryId == model.CategoryId &&
                                                                                         (item.AttributeControlTypeId == (int)AttributeControlType.DropdownList ||
                                                                                          item.AttributeControlTypeId == (int)AttributeControlType.RadioList)).ToList();
            //Remove the root from the list

            //Populate the Attribute Value List
            var tempAttributeValueList=new List<CategoryAttributeValuesViewModel>();

            if (model.CategoryAttributeId == -1)
            {
                if (tempCategoryAttributeList.Any())
                {
                    model.CategoryAttributeId = tempCategoryAttributeList.First().Id;
                    tempAttributeValueList = new CategoryAttributeValuesCore().GetMany(item => item.ClassifiedCategoryAttributeId == model.CategoryAttributeId).ToList();
                }
                
            }
            else
            {
                tempAttributeValueList = new CategoryAttributeValuesCore().GetMany(item => item.ClassifiedCategoryAttributeId == model.CategoryAttributeId).ToList();
            }
            

            //Populate the Category Attribute Value Object if it is already selected
            var tempAttributeValue =new CategoryAttributeValuesViewModel();

            if (model.AttributeValueId != -1)
            {
                //populate the form with the existence value of the attribute
                tempAttributeValue= new CategoryAttributeValuesCore().Get(item => item.Id == model.AttributeValueId);
            }
            else
            {
                if (model.AttributeValue != null && string.IsNullOrEmpty(model.AttributeValue.AttributeValue))
                {
                    tempAttributeValue = model.AttributeValue;
                }
            }

            var tempModel = new CategoryAttributeValuesManagementViewModelate()
            {
                CategoryList = tempCategoryList,
                CategoryId = model.CategoryId,
                CategoryAttributeId = model.CategoryAttributeId,
                AttributesList= tempCategoryAttributeList,
                AttributeValue = tempAttributeValue,
                AttributeValueId = model.AttributeValueId,
                AttributeValues = tempAttributeValueList
            };

            //Return the finally generated View Model
            return tempModel;
        }

        /// <summary>
        /// Populate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal CategoryAttributeValuesManagementViewModelate PopulateCategoryAttributeVluesViewModelUpdates(CategoryAttributeValuesManagementViewModelate model)
        {

            //Create a List for Categories that are published and Active
            var tempCategoryList = HirarchyList(new CategoriesCore()
                .GetMany(item => item.IsActive &&
                                 item.Published)
                .OrderBy(item => item.Name).ToList()).ToList();

            //Remove the Root from Category Lits
            tempCategoryList.RemoveAll(item => item.Id == -1);


            if (model.CategoryId == -1)
            {
                model.CategoryId = (tempCategoryList.FirstOrDefault() != null ? tempCategoryList.FirstOrDefault().Id : -1);
            }


            //Create a Sample View Model in order to send to View and populate it
            var tempCategoryAttributeList = new CategoryAttributesCore().GetMany(item => item.ClassifiedCategoryId == model.CategoryId &&
                                                                                         (item.AttributeControlTypeId == (int)AttributeControlType.DropdownList ||
                                                                                          item.AttributeControlTypeId == (int)AttributeControlType.RadioList)).ToList();
            //Remove the root from the list

            //Populate the Attribute Value List
            var tempAttributeValueList = new List<CategoryAttributeValuesViewModel>();

            if (model.CategoryAttributeId == -1)
            {
                if (tempCategoryAttributeList.Any())
                {
                    model.CategoryAttributeId = tempCategoryAttributeList.First().Id;
                    tempAttributeValueList = new CategoryAttributeValuesCore().GetMany(item => item.ClassifiedCategoryAttributeId == model.CategoryAttributeId).ToList();
                }

            }
            else
            {
                tempAttributeValueList = new CategoryAttributeValuesCore().GetMany(item => item.ClassifiedCategoryAttributeId == model.CategoryAttributeId).ToList();
            }


            //Populate the Category Attribute Value Object if it is already selected
            

            var tempModel = new CategoryAttributeValuesManagementViewModelate()
            {
                CategoryList = tempCategoryList,
                CategoryId = model.CategoryId,
                CategoryAttributeId = model.CategoryAttributeId,
                AttributesList = tempCategoryAttributeList,
                AttributeValue = model.AttributeValue,
                AttributeValueId = model.AttributeValueId,
                AttributeValues = tempAttributeValueList
            };

            //Return the finally generated View Model
            return tempModel;
        }


        /// <summary>
        /// Delete the Selected Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>To the category management page</returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        [Route("Admin/DeleteCategoryAttributeValue/{id}/{catAtrId}/{catId}", Name = "DeleteCategoryAttributeValue")]
        public ActionResult DeleteCategoryAttributeValue(int id, int catAtrId,int catId)
        {
            //Delete the record and return the result
            if (new CategoryAttributeValuesCore().Delete(item => item.Id == id))
            {
                //The Delete operation is successful show the success message
                //Show Error Message
                //Show the user that his or her password has been changed successfully.

                PopUpMessageGenerator.GenerateMessage("Category Attribute Values Management System",
                    "Selected Category Value deleted successfully.",
                    MessageStatus.Successfull);

                //Prepare from for new entry
                var tempModel = new CategoryAttributeValuesManagementViewModelate
                {
                    CategoryAttributeId = catAtrId,
                    CategoryId = catId,
                    AttributeValueId = -1
                };

                //Redirect to Attribute Management Page
                return RedirectToAction("CategoryAttributeValuesManager", PopulateCategoryAttributeVluesViewModelInsert(tempModel));
            }
            else
            {
                //The delete operation is not successful. Sow the failed message
                //Show Error Message
                //Show the user that his or her password has been changed successfully.

                PopUpMessageGenerator.GenerateMessage("Category Attribute Values Management System",
                    "There is a problem in deleting the selected category.",
                    MessageStatus.Failed);


                //Prepare from for new entry
                var tempModel = new CategoryAttributeValuesManagementViewModelate
                {
                    CategoryAttributeId = catAtrId,
                    CategoryId = catId,
                    AttributeValueId = -1
                };

                //Redirect to Attribute Management Page
                return View("CategoryAttributeValuesManager", PopulateCategoryAttributeVluesViewModelInsert(tempModel));

            }
        }

        #endregion

        #region Advertisement Management Related Actions

        /// <summary>
        /// Email Based Ads Confirmation View Loader
        /// </summary>
        /// <returns>The View of Advertisement Manager</returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult EmailBasedAdsReview(ClassifiedAdvertismentManagementViewModel model)
        {
            //Load the List of Target Advertisements
            var adsManager = new ClassifiedAdvertisementCore();
            model.AdsList = adsManager.GetMany(item => item.IsEmailBase && !(item.IsApproved || item.IsRejected));
             
            if (model.Id== -1)
            {
                //Initial Load of information
                return View(model);
            }

            //Load the information related to the Advertisement
            model.ClassifiedAds = adsManager.GetAdsFullInfo(model.Id);

            
            return View(model);
        }

        /// <summary>
        /// Confirm the advertisement
        /// </summary>
        /// <param name="Id">Id of the advertisement</param>
        /// <returns>To the form of Email based advertisement</returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult FinalConfirmAdsEmail(long Id)
        {
            try
            {
                var adsManager = new ClassifiedAdvertisementCore();
                //Retrieve the information of the target Classified Ads
                var tempAds = adsManager.Get(item => item.Id == Id);

                if (tempAds != null)
                {
                    //Set the Confirm Flags
                    tempAds.IsApproved = true;
                    //Since ads approved set the flag of isRejected to false
                    tempAds.IsRejected = false;
                    // make the advertisement Active
                    tempAds.IsActive = true;
                    // Since the advertisement reviewed once. There is no need for this flag to be true
                    tempAds.HasReviewPriority = false;

                    // Set the update to the current date and time so the user is going to be able to promote this ad after 48 hours.
                    tempAds.UpdatedOnUtc = DateTime.UtcNow;

                    // Set the user Id of the reviewer who approved this advertisement.
                    tempAds.ReviewedByUser = HttpContext.User.Identity.GetUserId();

                    //Update Database
                    if (adsManager.Update(tempAds, item => item.Id == Id))
                    {
                        //Send email to user about approvement of the email address.
                        if (new EmailService().EmailBasedAdvertisementApprovement(tempAds.Id, tempAds.EmailAddress))
                        {
                            //Show the Success Message
                            PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                "The target Advertisement has been approved successfully.",
                                MessageStatus.Successfull);
                        }
                        else
                        {
                            //Show Alert about not sending the email
                            PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                "The target Advertisement has been approved. However, for some reason, it is impossible to send the approvement email at the moment.",
                                MessageStatus.Warning);
                        }

                        return RedirectToAction("EmailBasedAdsReview", new ClassifiedAdvertismentManagementViewModel());

                    }
                    else
                    {
                        PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                            "There is a problem on updating information of target advertisement.",
                            MessageStatus.Failed);

                        var tempModel = new ClassifiedAdvertismentManagementViewModel
                        {
                            Id =Id
                        };

                        return RedirectToAction("EmailBasedAdsReview", tempModel);
                    }

                }
                else
                {
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "We did not find the target advertisement.",
                        MessageStatus.Warning);

                    var tempModel = new ClassifiedAdvertismentManagementViewModel
                    {
                        Id = Id
                    };

                    return RedirectToAction("EmailBasedAdsReview", tempModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "There is a run time error in the process of advertisement approvement.",
                    MessageStatus.Failed);

                var tempModel = new ClassifiedAdvertismentManagementViewModel
                {
                    Id = Id
                };

                return RedirectToAction("EmailBasedAdsReview", tempModel);
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.AdvancedUser)]
        public ActionResult AdsEmailRejection(ClassifiedAdvertisementRejectCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var tempModel = new ClassifiedAdvertismentManagementViewModel
                {
                    Id = model.ClassifiedAdvertisementId
                };

                return View("EmailBasedAdsReview", tempModel);
            }

            try
            {
                //Add the comment to the rejection reasons of this advertisement
                var adsManager = new ClassifiedAdvertisementCore();

                var tempAds = adsManager.Get(item => item.Id == model.ClassifiedAdvertisementId);

                if (tempAds != null)
                {
                    //Set the flags
                    //Set the Confirm Flags
                    tempAds.IsApproved = false;
                    //Since ads approved set the flag of isRejected to false
                    tempAds.IsRejected = true;
                    // make the advertisement Active
                    tempAds.IsActive = false;
                    // Since the advertisement reviewed once. There is no need for this flag to be true
                    tempAds.HasReviewPriority = true;

                    // Set the user Id of the reviewer who approved this advertisement.
                    tempAds.ReviewedByUser = HttpContext.User.Identity.GetUserId();

                    model.RejectedByUser= HttpContext.User.Identity.GetUserId();

                    var commentManager = new ClassifiedAdvertisementRejectCommentCore();

                    var tempComment =commentManager.InsertByReturningTargetObject(model);

                    //Add the rejection reason
                    if (tempComment.Id>0)
                    {
                        //Update the advertisement
                        if (adsManager.Update(tempAds, item => item.Id == model.ClassifiedAdvertisementId))
                        {
                            //Send the Rejection Email to the user
                            if (new EmailService().EmailBasedAdvertisementRejection(model.ClassifiedAdvertisementId, tempAds.EmailAddress))
                            {
                                //Show the Success Message to user
                                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                    "This advertisement rejected successfully.",
                                    MessageStatus.Successfull);
                            }
                            else
                            {
                                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                    "This advertisement rejected successfully.However, we failed to send the rejection email.",
                                    MessageStatus.Warning);
                            }

                            var tempModel = new ClassifiedAdvertismentManagementViewModel();
                            
                            return RedirectToAction("EmailBasedAdsReview", tempModel);
                        }
                        else
                        {
                            //Delete the rejection reason
                            PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                commentManager.Delete(item => item.Id == tempComment.Id)
                                    ? "We failed to update this advertisement and we deleted the rejection reasons you provided."
                                    : "There is a serious failure in our system. Please contact development team.",
                                MessageStatus.Failed);

                            var tempModel = new ClassifiedAdvertismentManagementViewModel
                            {
                                Id = model.ClassifiedAdvertisementId
                            };

                            return RedirectToAction("EmailBasedAdsReview", tempModel);

                        }
                    }
                    else
                    {
                        //Show not comment registration error
                        //Show the serious failure in the process
                        PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                            "System failed to submit the rejection reason.",
                            MessageStatus.Failed);

                        var tempModel = new ClassifiedAdvertismentManagementViewModel
                        {
                            Id = model.ClassifiedAdvertisementId
                        };

                        return RedirectToAction("EmailBasedAdsReview", tempModel);
                    }

                }
                else
                {
                    //Not found error

                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "We did not find the target advertisement.",
                        MessageStatus.Warning);

                    var tempModel = new ClassifiedAdvertismentManagementViewModel
                    {
                        Id = model.ClassifiedAdvertisementId
                    };

                    return RedirectToAction("EmailBasedAdsReview", tempModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "There is a run time error in the process of advertisement approvement.",
                    MessageStatus.Failed);

                var tempModel = new ClassifiedAdvertismentManagementViewModel
                {
                    Id = model.ClassifiedAdvertisementId
                };

                return RedirectToAction("EmailBasedAdsReview", tempModel);
            }
            
        }

        #endregion

        #endregion
    }

}
