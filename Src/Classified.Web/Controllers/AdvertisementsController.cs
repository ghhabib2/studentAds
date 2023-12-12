using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Classified.Component.Html;
using Classified.Data.Advertisements.Advertisement;
using Classified.Data.Advertisements.Categories;
using Classified.Data.Advertisements.Images;
using Classified.Domain.ViewModels;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Services;
using Classified.Services.Advertisement;
using Classified.Services.DataCache.Advertisement.Categories;
using Classified.Services.Security;
using Classified.Web.UserServices;
using Microsoft.AspNet.Identity;

namespace Classified.Web.Controllers
{
    public class AdvertisementsController : Controller
    {

        #region Advertisement Related Actions and Methods



        /// <summary>
        /// Action for confirmation of Advertisement
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmailBaseAdsConfirmation(string email, string token)
        {
            //Check if the token is valid
            if (ClassifiedTokenProvieser.ValidateToken(token, 24))
            {
                //Token is valid. Confirm the email address
                var tempAdsInfo = new ClassifiedAdvertisementConfirmationCore().GetAdvertiement(email, token);

                if (tempAdsInfo != null)
                {
                    //Check if the advertisement confirmed before
                    if (tempAdsInfo.IsEmailConfirmed)
                    {
                        //Send a message showing that this advertisement Confirmed before
                        PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                            $"Dear {email}, this ads is already confirmed.",
                            MessageStatus.Warning);

                        var tempModel = new EmailBaseAdsConfirmationViewModel
                        {
                            EmailAddress = email,
                            EmailLink = new Services.Advertisement.EmailService().ModificationLinkGenerator(email, tempAdsInfo.Id)
                        };
                        //Show the View
                        return View(tempModel);
                    }
                    else
                    {
                        //Confirm email address
                        tempAdsInfo.IsEmailConfirmed = true;
                        tempAdsInfo.IsEmailBase = true;

                        //Confirm email address and ads
                        if (new ClassifiedAdvertisementCore().Update(tempAdsInfo, m => m.Id == tempAdsInfo.Id))
                        {
                            //Delete the record from confirmation db
                            if (new ClassifiedAdvertisementConfirmationCore().Delete(m =>
                                m.EmailAddress == email && m.ProvidedToken == token))
                            {
                                //Create another email with message for editing email address based on ads Id
                                if (new Services.Advertisement.EmailService().AdvertisementConfirmationEmail(email,
                                    tempAdsInfo.Id))
                                {
                                    var tempModel = new EmailBaseAdsConfirmationViewModel
                                    {
                                        EmailAddress = email,
                                        EmailLink = new Services.Advertisement.EmailService().ModificationLinkGenerator(email, tempAdsInfo.Id)
                                    };
                                    //Show the View
                                    return View(tempModel);
                                }
                                else
                                {
                                    //Add a confirmation message
                                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                        $"Dear {email}, we confirmed your email address, but it is impossible for us to send the confirmation email and the link to you right now. Please save the link we sent you in this page in order to make sure you would not loose access to your submitted advertisement.",
                                        MessageStatus.Warning);

                                    var tempModel = new EmailBaseAdsConfirmationViewModel
                                    {
                                        EmailAddress = email,
                                        EmailLink = new Services.Advertisement.EmailService().ModificationLinkGenerator(email, tempAdsInfo.Id)
                                    };
                                    //Show the View
                                    return View(tempModel);
                                }

                            }
                            else
                            {
                                //Show a message that the email is confirmed but the record is not deleted
                                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                    $"Dear {email}, we confirmed your email address, But there are some problems relating our processes. Please make sure that you save the content of this page in order no to loose your access to your advertisment.",
                                    MessageStatus.Warning);

                                //Show the View
                                var tempModel = new EmailBaseAdsConfirmationViewModel
                                {
                                    EmailAddress = email,
                                    EmailLink = new Services.Advertisement.EmailService().ModificationLinkGenerator(email, tempAdsInfo.Id)
                                };
                                //Show the View
                                return View(tempModel);
                            }

                        }
                        else
                        {
                            //Show error message
                            PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                                $"Dear {email}, there is a problem in confirmation process. Please wait a few minutes and try again. Please contact our development team to solve the problem if you see this message again",
                                MessageStatus.Failed);

                            //Redirect to Home
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            else
            {
                //Token is not valid. Check if there is a record with this information and delete the advertisement.
                var tempAdsInfo = new ClassifiedAdvertisementConfirmationCore().GetAdvertiement(email, token);

                if (new ClassifiedAdvertisementCore().Delete(m => m.Id == tempAdsInfo.Id))
                {
                    //Show the message
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        $"Dear {email}, this advertisement is not valid any more. We deleted the information  related to it. Please resubmit the advertisement information and make sure that you confirm your add within 24 hours time limitation.",
                        MessageStatus.Warning);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Show error message
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        $"Dear {email}, this advertisement is not valid any more. However, we detected some sort of problem removing your advertisement information from our database. Your current advertisement information will be deleted from our database as soon as we find the problem. Thereupon, please resubmit this advertisement and make sure you confirm it within 24 hours.",
                        MessageStatus.Warning);

                    return RedirectToAction("Index", "Home");
                }

            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Store information of email based submitted Advertisement in database
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult SubmitByEmail(AdvertisementEmailBasePrimaryRegisterationViewModel model)
        {
            //Check the Google reCapcha to see if the user passed the Google reCapcha or not

            if (!CaptchaValidator.isValidated)
            {
                ModelState.AddModelError("reCaptchaValidation", "Please pass the Google's ReCaptcha validation by checking the box \"I'm not a robot\"");

                //Populate the model
                PopulateAdvertisementEmailBasePrimaryRegisterationViewModel(ref model);

                return View("SubmitAdsByEmail", model);
            }

            //Validate the model

            if (!ModelState.IsValid)
            {
                //Populate the model
                PopulateAdvertisementEmailBasePrimaryRegisterationViewModel(ref model);

                return View("SubmitAdsByEmail", model);
            }


            //Set the parameters of Model that would not be exist in time of submitssion.
            model.SubmitDate = DateTime.UtcNow;
            model.CreatedOnUtc = DateTime.UtcNow;
            model.UpdatedOnUtc = DateTime.UtcNow;
            model.IsEmailConfirmed = false;
            model.IsEmailBase = true;

            //Validation Complete Submit the advertisement
            var tempAdsData = new ClassifiedAdvertisementEmailPrimaryCore().InsertByReturningTargetObject(model);

            if (tempAdsData != null)
            {
                var tempTokenViewModel = new AdvertisementConfirmationViewModel
                {
                    ClassifiedAdvertisementId = tempAdsData.Id,
                    EmailAddress = model.EmailAddress,
                    ProvidedToken = ClassifiedTokenProvieser.NewToken
                };

                if (new ClassifiedAdvertisementConfirmationCore().Insert(tempTokenViewModel))
                {
                    if (new Services.Advertisement.EmailService().EmailSubmitConfirmation(model.EmailAddress,
                        tempTokenViewModel.ProvidedToken))
                    {
                        PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                             "Your advertisement has been submitted successfully. Please confirm this advertisement within 24 hours using a confirmation link sent to your Inbox. Please check the Spam box in the case you would not find the email in your Inbox.",
                             MessageStatus.Successfull);

                        //Return to the Index
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        //Add a confirmation message
                        PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                            "Your advertisement has been submitted successfully. However, it is impossible for us to send the confirmation email right away. We will send this email as soon as it is possible for us. Please confirm this advertisement within 24 hours using a confirmation link sent to your Inbox. Please check the Spam box in the case you would not find the email in your Inbox.",
                            MessageStatus.Warning);

                        //Return to the Index
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {

                    //Drop the record from advertisement table
                    new ClassifiedAdvertisementEmailPrimaryCore().Delete(item => item.Id == tempAdsData.Id);

                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "There is a problem on submitting your advertisement. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                        MessageStatus.Failed);

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                //Add a confirmation message
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "There is a problem on submitting your advertisement. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                    MessageStatus.Failed);

                //End
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult SubmitAdsByEmail()
        {
            //Create a sample view model by default
            var tempModel = new AdvertisementEmailBasePrimaryRegisterationViewModel();

            //Populate the model
            PopulateAdvertisementEmailBasePrimaryRegisterationViewModel(ref tempModel);

            return View(tempModel);
        }

        /// <summary>
        /// Populate the model sent by the system and make sure it has primary essential informatoin
        /// </summary>
        /// <param name="model"></param>
        protected internal void PopulateAdvertisementEmailBasePrimaryRegisterationViewModel(
            ref AdvertisementEmailBasePrimaryRegisterationViewModel model)
        {
            //Create a new Hierarchy list
            var tempCategoriesHirarchyList = HirarchyList(new CategoriesCore()
                .GetMany(item => item.Published &&
                                 item.Deleted == false &&
                                 item.IsActive).OrderBy(item => item.Name).ToList()).ToList();

            model.Categories = tempCategoriesHirarchyList;

            if (model.ClassifiedCategoryId.HasValue && model.ClassifiedCategoryId.Value != -1)
            {
                model.ClassifiedCategoryId = model.ClassifiedCategoryId;
            }
            else
            {
                model.ClassifiedCategoryId = -1;
            }

            //Set the initial values to other items
            model.Id = -1;


        }


        /// <summary>
        /// Get the List of Categories and convert it to Presentable Hierarchy List for View Purpose
        /// </summary>
        /// <param name="basicList"></param>
        /// <returns></returns>
        protected internal IEnumerable<CategoryItemClientViewMoel> HirarchyList(List<CategoryViewMoel> basicList)
        {


            //Select the Parent Nodes.
            var categoryViewMoels = basicList.ToList();
            var parentNodes = categoryViewMoels.Where(c => c.ParentCategory == null).ToList();

            var outputList = new List<CategoryItemClientViewMoel>();

            foreach (var category in parentNodes)
            {

                //Check for children nodes
                if (categoryViewMoels.Count(c => c.ParentCategoryId == category.Id) > 0)
                {
                    //Add the Item to the List

                    var tempItem = new CategoryItemClientViewMoel()
                    {
                        Name = category.Name,
                        Id = category.Id.ToString()
                    };

                    //Add the children to the list
                    AppendChildren(ref tempItem, ref basicList);

                    outputList.Add(tempItem);

                }
                else
                {

                    var tempItem = new CategoryItemClientViewMoel()
                    {
                        Name = category.Name,
                        Id = category.Id.ToString(),
                        ChildCategories = new List<CategoryItemClientViewMoel>()
                    };

                    outputList.Add(tempItem);

                }
            }

            return outputList;
        }

        /// <summary>
        /// Add the child nodes to the list of parent nodes
        /// </summary>
        /// <param name="parentItem">Parent Node</param>
        /// <param name="basicList"></param>
        protected internal void AppendChildren(ref CategoryItemClientViewMoel parentItem, ref List<CategoryViewMoel> basicList)
        {

            //Assign Parent Category to a local variable to use
            var parentCategoryId = Convert.ToInt32(parentItem.Id);

            //Fetch the Child List
            var childList = basicList.Where(c => c.ParentCategoryId == parentCategoryId).ToList();


            foreach (var categoryItem in childList)
            {
                //Check for children nodes
                if (basicList.Count(c => c.ParentCategoryId == parentCategoryId) > 0)
                {

                    var tempItem = new CategoryItemClientViewMoel
                    {
                        Name = categoryItem.Name,
                        Id = categoryItem.Id.ToString()
                    };

                    AppendChildren(ref tempItem, ref basicList);

                    parentItem.Id = (-1).ToString();
                    parentItem.ChildCategories.Add(tempItem);

                }
                else
                {

                    var tempItem = new CategoryItemClientViewMoel
                    {
                        Name = categoryItem.Name,
                        Id = categoryItem.Id.ToString(),
                        ChildCategories = new List<CategoryItemClientViewMoel>()
                    };

                    parentItem.ChildCategories.Add(tempItem);
                }
            }
        }

        #endregion

        #region Advertisements Modification Related Actions and Methods

        /// <summary>
        /// Open View for the user to Manage the Advertisment
        /// </summary>
        /// <param name="id">Id of the Advertisement</param>
        /// <param name="email">Email of advertisement owner</param>
        /// <param name="state">State of the Form</param>
        /// <returns></returns>
        [Route("Advertisements/AdsEmailModification/{email}/{id:long}/{state:int}", Name = "AdsEmailModification")]
        public ActionResult AdsEmailModification(long id, string email, int state)
        {
            //Check if the Data Exist in database
            var adsManager = new ClassifiedAdvertisementModificationCore();
            var tempModel = adsManager.Get(item =>
                item.Id == id && item.EmailAddress == email);

            if (tempModel != null)
            {
                //Populate the form
                tempModel.IsFillData = false;

                //Populate the rest of view model
                PopulateAdvertisementModificationViewModel(ref adsManager, ref tempModel, id);

                //Set the State to the very first tab which is Category Update 
                switch (state)
                {
                    case 1:
                        {
                            tempModel.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateCategory;
                            break;
                        }
                    case 2:
                        {
                            tempModel.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdatePrimaryData;
                            break;
                        }
                    case 3:
                        {
                            tempModel.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateAttributes;
                            break;
                        }
                    case 4:
                        {
                            tempModel.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateImages;
                            break;
                        }
                    case 5:
                        {
                            tempModel.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateDescription;
                            break;
                        }
                    case 6:
                    {
                        tempModel.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateSeo;
                        break;
                    }
                }

                //if (tempModel.IsSubmitted)
                //{
                //    //Show warning message to the user
                //    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                //        "This form has already submitted.",
                //        MessageStatus.Warning);
                //}

                //Load the Modification Form
                return View("AdsModificationForm", tempModel);
            }
            else
            {
                //Return Default not Found Error
                return HttpNotFound();
            }

        }

        /// <summary>
        /// Modify the Category of the Advertisement
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCategory(ClassifiedAdvertisementModifyViewModel model)
        {
            //Validate the Model
            //if (!ModelState.IsValid)
            //{
            //    //Temporary store the information of category Id
            //    var tempCategroyId = model.ClassifiedCategoryId;
            //    //Set the Flag of Fill form to be true
            //    model.IsFillData = true;
            //    // Re-Populate the form based on the information submitted
            //    PopulateAdvertisementModificationViewModel(ref model, model.Id);

            //    //Populate the Category Id again
            //    model.ClassifiedCategoryId = tempCategroyId;

            //    //Set the flag of the Tab to be displayed
            //    model.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateCategory;

            //    return View("AdsModificationForm", model);
            //}

            var adsManager = new ClassifiedAdvertisementModificationCore();

            if (model.ClassifiedCategoryId == -1)
            {
                ModelState.AddModelError("You cannot select a Category from the list of parent categories.", new Exception());
                //Temporary store the information of category Id
                var tempCategroyId = model.ClassifiedCategoryId;
                //Set the Flag of Fill form to be true
                model.IsFillData = true;
                // Re-Populate the form based on the information submitted
                
                PopulateAdvertisementModificationViewModel(ref adsManager, ref model, model.Id);

                //Populate the Category Id again
                model.ClassifiedCategoryId = tempCategroyId;

                //Set the flag of the Tab to be displayed
                model.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateCategory;

                return View("AdsModificationForm", model);
            }

            //Model is Valid. Update the CategoryId
            if (adsManager.UpdateCategory(item => item.Id == model.Id,
                model.ClassifiedCategoryId.Value))
            {
                //Repopulate the Model
                model.IsFillData = true;
                
                PopulateAdvertisementModificationViewModel(ref adsManager, ref model, model.Id);
                //Generate the Success message
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "The Category of your Advertisement has been updates successfully",
                    MessageStatus.Successfull);


            }
            else
            {
                //Repopulate the Model
                model.IsFillData = true;
                PopulateAdvertisementModificationViewModel(ref adsManager,ref model, model.Id);
                //Generate Error Message
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "There is a problem in updating your advertisement category. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                    MessageStatus.Failed);
            }

            //Redirect to the View
            return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 1 });
        }

        /// <summary>
        /// Populate the model sent by the system and make sure it has primary essential information
        /// </summary>
        /// <param name="adsManager">Core for advertisement Modification</param>
        /// <param name="model"></param>
        /// <param name="id"></param>
        protected internal void PopulateAdvertisementModificationViewModel(ref ClassifiedAdvertisementModificationCore adsManager,
            ref ClassifiedAdvertisementModifyViewModel model, long id)
        {
            //Check if the population Method should fill the information of form as well?
            if (model.IsFillData)
            {
                //Fill the Data
                model = adsManager.Get(item => item.Id == id);
            }

            //Create a new Hierarchy list
            var tempCategoriesHirarchyList = HirarchyList(new CategoriesCore()
                .GetMany(item => item.Published &&
                                 item.Deleted == false &&
                                 item.IsActive).OrderBy(item => item.Name).ToList()).ToList();

            model.ClassifiedCategoryList = tempCategoriesHirarchyList;

            if ((!model.ClassifiedCategoryId.HasValue) && model.ClassifiedCategoryId.Value == -1)
            {
                model.ClassifiedCategoryId = -1;
            }

            //Fill the list of Attributes
            model.AdvertisementAttributes = PopulateAdvertismentAttributes(ref adsManager,model.Id).ToList();

            //Rejection List
            model.RejectionReasonsLIst = PopulateAdvertismentRejectionList(ref adsManager, model.Id).ToList();

            //Fill the information of Images
            model.Images = new ImageCore().GetMany(item => item.ClassifiedAdvertisementId == id).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBasicInformation(ClassifiedAdvertisementModifyViewModel model)
        {
            //Validate the Model
            if (!ModelState.IsValid)
            {
                //Set the Flag of Fill form to be true
                model.IsFillData = false;
                // Re-Populate the form based on the information submitted
                var adsManager = new ClassifiedAdvertisementModificationCore();
                PopulateAdvertisementModificationViewModel(ref adsManager, ref model, model.Id);

                //Set the flag of the Tab to be displayed
                model.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdatePrimaryData;

                return View("AdsModificationForm", model);
            }

            var modeltoUpdate = new ClassifiedAdvertisementModificationCore().Get(item => item.Id == model.Id);

            if (modeltoUpdate != null)
            {
                //Update the basic information
                modeltoUpdate.Title = model.Title;
                modeltoUpdate.Address = model.Address;
                modeltoUpdate.ShortDescription = model.ShortDescription;
                modeltoUpdate.PhoneNumber = model.PhoneNumber;
                modeltoUpdate.Price = model.Price;
            }

            //set the submit Date
            model.SubmitDate = DateTime.UtcNow;

            //Model is Valid. Update Information
            if (new ClassifiedAdvertisementModificationCore().Update(modeltoUpdate, item => item.Id == model.Id))
            {
                //Advertisement record has been updated successfully.
                //Generate the Success Message
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "Your advertisement basic information has been updated successfully.",
                    MessageStatus.Successfull);
            }
            else
            {
                //There is a problem in update process of basic information.
                //Generate the Failed Message
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "There is a problem in updating your advertisement basic information. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                    MessageStatus.Failed);
            }

            //Redirect to the Route
            return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 2 });
        }

        /// <summary>
        /// Populate the Advertisement Attributes View Model
        /// </summary>
        /// <param name="adsManager">Core for Modification Advertisement View Model</param>
        /// <param name="advertisementId">Advertisement Id</param>
        /// <returns></returns>
        protected internal IEnumerable<AdvertisementAttributesViewModel> PopulateAdvertismentAttributes(ref ClassifiedAdvertisementModificationCore adsManager, long advertisementId)
        {
            //Fetch the list of current Attributes List
            return adsManager.PopulateAttributes(item => item.Id == advertisementId);
        }

        /// <summary>
        /// Populate the Advertisement Attributes View Model
        /// </summary>
        /// <param name="adsManager">Core for Modification Advertisement View Model</param>
        /// <param name="advertisementId">Advertisement Id</param>
        /// <returns></returns>
        protected internal IEnumerable<ClassifiedAdvertisementRejectCommentViewModel> PopulateAdvertismentRejectionList(ref ClassifiedAdvertisementModificationCore adsManager, long advertisementId)
        {
            //Fetch the list of current Attributes List
            return adsManager.GetRejectionFullList(advertisementId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAttributes()
        {
            //Read the information of the attributes based on the categroyId
            if (Request["ClassifiedCategoryId"] != null)
            {
                //Get the ClassifiedCategoryId from the list of parameters
                var tempClassifiedCategoryId = Convert.ToInt32(Request["ClassifiedCategoryId"]);

                //Load the list of attributes
                var tempAttributesList =
                    new CategoryAttributesCore().GetMany(item => item.ClassifiedCategoryId == tempClassifiedCategoryId);

                //Create an attribute view model
                var tempAdsAttributesList = new List<AdvertisementAttributesValueViewModel>();

                //Surf the items in categories attribute list
                foreach (var attribute in tempAttributesList)
                {
                    //Find out if the related request has been send to the form
                    if (Request[attribute.AttributeName] != null)
                    {
                        //Add the attribute information to the ads attributelist
                        tempAdsAttributesList.Add(new AdvertisementAttributesValueViewModel
                        {
                            ClassifiedAdvertisementId = Convert.ToInt64(Request["Id"]),
                            ClassifiedCategoryAttributeId = attribute.Id,
                            Value = Request[attribute.AttributeName]
                        });
                    }
                }

                //Insert data to Database
                if (new AdvertisementAttributesCore().UpdateAdsAttributes(tempAdsAttributesList,
                    Convert.ToInt64(Request["Id"]),
                    Convert.ToInt32(Request["ClassifiedCategoryId"])))
                {
                    //The operation is successful show the success message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "Your advertisement attribute information has been updated successfully.",
                        MessageStatus.Successfull);
                    //Redirect to the Route
                    return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"], state = 3 });
                }
                else
                {
                    //There is a problem on inserting information into the database. Show the failed message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "There is a problem in advertisement attribute information saving process. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                        MessageStatus.Successfull);

                    //Redirect to the Route
                    return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"] });

                }



            }

            return null;
        }

        /// <summary>
        /// Update the description of the Advertisement
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDescripiton(AdvertismentDescriptionViewModel model)
        {

            //Start Updating the Description
            var tempAdsModel = new ClassifiedAdvertisementCore().Get(item => item.Id == model.Id);

            if (tempAdsModel != null)
            {
                //Set the Description of the Model
                tempAdsModel.Description = model.Description;

                //Update Database
                if (new ClassifiedAdvertisementCore().Update(tempAdsModel, item => item.Id == model.Id))
                {
                    //Show Success Message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "The description of your advertisement has been updated successfully.",
                        MessageStatus.Successfull);

                    //Return to the View
                    return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 5 });
                }
                else
                {
                    //Show Failed message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "There is a problem in the process of updating your advertisement description. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                        MessageStatus.Failed);

                    //Return to the View
                    return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 5 });
                }
            }
            else
            {
                //Show record not found error to the user
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "We are not able to find the information related to your advertisement.",
                    MessageStatus.Warning);

                //Return to the View
                return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 5 });
            }

        }

        /// <summary>
        /// Update the SEO information
        /// </summary>
        /// <param name="model"></param>
        /// <returns>To View after the operation</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSeo(AdvertisementSeoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var tempModel = new ClassifiedAdvertisementModifyViewModel { IsFillData = true };
                //Load the model again and return back to the form
                var seoModel = new AdvertisementSeoViewModel
                {
                    Id = model.Id,
                    EmailAddress = model.EmailAddress,
                    MetaTitle = model.MetaTitle,
                    MetaKeyWord = model.MetaKeyWord,
                    MetaDescription = model.MetaDescription
                };
                var adsManager = new ClassifiedAdvertisementModificationCore();

                PopulateAdvertisementModificationViewModel(ref adsManager, ref tempModel, model.Id);

                //Refill the information of the form related to SEO 
                tempModel.MetaTitle = seoModel.MetaTitle;
                tempModel.MetaDescription = seoModel.MetaDescription;
                tempModel.MetaKeyWord = seoModel.MetaKeyWord;
                tempModel.AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateSeo;

                return View("AdsModificationForm", tempModel);

            }

            //Update the information related to Model

            //Fetch the information of the Advertisement
            var adsModel = new ClassifiedAdvertisementCore().Get(item => item.Id == model.Id);

            if (adsModel != null)
            {
                //do Update
                adsModel.MetaTitle = model.MetaTitle;
                adsModel.MetaKeyWord = model.MetaKeyWord;
                adsModel.MetaDescription = model.MetaDescription;

                if (new ClassifiedAdvertisementCore().Update(adsModel, item => item.Id == model.Id))
                {
                    //Information updated
                    //Show Success Message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "The SEO of your advertisement has been updated successfully.",
                        MessageStatus.Successfull);

                    //Return to the View
                    return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 6 });
                }
                else
                {
                    //Update operation failed
                    //Show Failed Message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "There is a problem in updating the SEO information. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                        MessageStatus.Failed);

                    //Return to the View
                    return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 6 });
                }
            }
            else
            {
                //Alert user about not finding information
                //Show Success Message to the user
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "We did not find information related to your advertisement. Please make sure that you guided throw this page using a authorized link",
                    MessageStatus.Warning);

                //Return to the View
                return RedirectToRoute("AdsEmailModification", new { id = model.Id, email = model.EmailAddress, state = 6 });
            }
        }

        /// <summary>
        /// Return the Classified Advertisement Single page or its preview
        /// </summary>
        /// <returns></returns>
        [Route("Advertisements/classifiedAd/{id:long}", Name = "classifiedAdsDisplay")]
        public ActionResult ClassifiedAd(long id)
        {
            //Load the information of the Classified Advertisement
            var tempAds = new ClassifiedAdvertisementCore().GetAdsFullInfoClient(id);

            //Check if the engine successfully retrieve the classified Advertisement
            if (tempAds != null)
            {
                //Set Preview related Flags
                tempAds.IsPreview = true;
                tempAds.IsActive = false;

                //Return to View
                return View("ClassifiedAd", tempAds);
            }
            else
            {
                //Show HTTP Not Found Error
                return HttpNotFound();
            }
        }


        /// <summary>
        /// Return the Classified Advertisement Single page or its preview
        /// </summary>
        /// <returns></returns>
        [Route("Advertisements/classifiedAds/{id:long}", Name = "AdsDisplay")]
        public ActionResult ClassifiedAds(long id)
        {
            //Load the information of the Classified Advertisement
            var tempAds = new ClassifiedAdvertisementCore().GetAdsFullInfoClient(id);

            //Check if the engine successfully retrieve the classified Advertisement
            if (tempAds != null)
            {
                //Load breadcrumbs
                tempAds.BreadcrumbList = PopulateAdversiementBreadcrumb(tempAds);
                
                //Return to View
                return View("ClassifiedAd", tempAds);
            }
            else
            {
                //Show HTTP Not Found Error
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Do the submission and send the Advertisement for review
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClassifiedAdvertisementEmailSubmission()
        {
            //Validate the parameters
            if (Request["Id"] == null && Request["EmailAddress"] == null)
            {
                //Show warning message to the user
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "You directed to this operation using an incorrect link. Please make sure that you are using a validated link.",
                    MessageStatus.Warning);

                //Return to the Advertisement Modification Page
                return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"], state = 1 });
            }

            var adsId = Convert.ToInt64(Request["Id"]);

            //Select the Classified Advertisement View Model
            var adsManager = new ClassifiedAdvertisementCore();
            var tempModel = adsManager.Get(item => item.Id == adsId);

            if (tempModel != null)
            {
                if (tempModel.IsSubmitted && !(tempModel.IsApproved || tempModel.IsRejected))
                {
                    //Show warning message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "This form has already submitted.",
                        MessageStatus.Warning);

                    //Return to the Advertisement Modification Page
                    return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"], state = 1 });
                }

                //Sett the flags that is going to be updated in the database
                tempModel.IsSubmitted = true;
                tempModel.IsEmailBase = true;
                tempModel.IsApproved = false;
                tempModel.IsRejected = false;

                tempModel.SubmitDate = DateTime.UtcNow;

                if (adsManager.Update(tempModel, item => item.Id == adsId))
                {
                    //Send the notification email to user
                    if (new EmailService().AdvertisementFinalSubmission(adsId, tempModel.EmailAddress,
                        AppSettings.SiteName))
                    {
                        //Show Related Message to the User
                        PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                            "Your advertisement has been submitted successfully for review. You will receive final status related to your ads by email. Thank you again for choosing our service.",
                            MessageStatus.Successfull);
                    }
                    else
                    {
                        //Show Related Message to the User
                        PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                            "Your advertisement has been successfully submitted for final review. However, for some reason we are not able to send you a confirmation email at the moment. You will recieve any update related to this advertisement final status via email. Thank you again for choosing our service.",
                            MessageStatus.Warning);
                    }

                }
                else
                {
                    //Show the Error Message to the user
                    PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                        "There is a problem is the process of submission this advertisement. Please wait a few minutes and try again. Please contact our development team if you see this message agian.",
                        MessageStatus.Failed);

                    //Return to the Advertisement Modification Page
                    return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"], state = 1 });
                }
            }
            else
            {
                //Show not found message to the user
                PopUpMessageGenerator.GenerateMessage("Advertisement Management System",
                    "We did not find the advertisement you are about to submit. Please make sure that you sent this request throw a validated link",
                    MessageStatus.Warning);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Populate the breadcrumb of the page
        /// </summary>
        /// <param name="classifiedAds">Classified Ads that loaded</param>
        /// <returns></returns>
        protected internal IEnumerable<BreadcrumbViewModel> PopulateAdversiementBreadcrumb(ClassifiedAdvertisementViewModelClient classifiedAds)
        {

            var finalResult=new List<BreadcrumbViewModel>();

            //Add Home
            finalResult.Add(new BreadcrumbViewModel {LinkName = "Home",LInkUrl = Url.Action("Index","Home")});

            //Add Categories
            finalResult.Add(new BreadcrumbViewModel {LinkName = "Categories",LInkUrl = Url.RouteUrl("CategoriesHome")});

            //Create a list of categories and their parents
            //=====================================================
            var tempParentsList=new List<CategoryItemViewMoel>();
            ParentCategoryLoad(classifiedAds.ClassifiedCategoryId, ref tempParentsList);

            //Check if there is a record in parents list
            if (tempParentsList.Any())
            {
                foreach (var categoryItem in tempParentsList.ToArray().Reverse())
                {
                    //Add Categories
                    finalResult.Add(new BreadcrumbViewModel { LinkName = categoryItem.Name, LInkUrl = Url.RouteUrl("CategoryHome",new {id=categoryItem.Id})});
                }
            }

            //Add Current Category
            finalResult.Add(new BreadcrumbViewModel { LinkName = classifiedAds.ClassifiedCategory.Name, LInkUrl = Url.RouteUrl("CategoryHome", new { id = classifiedAds.ClassifiedCategoryId }) });

            //Add the Advertisement itself
            finalResult.Add(new BreadcrumbViewModel { LinkName = classifiedAds.Title, LInkUrl = "#"});

            return finalResult;
        }

        /// <summary>
        /// Load the parent categories into the list
        /// </summary>
        /// <param name="categoryId">CategoryId</param>
        /// <param name="parentsList">ParentList</param>
        /// <returns></returns>
        protected internal void ParentCategoryLoad(int categoryId,ref List<CategoryItemViewMoel> parentsList)
        {
            //load categories list
            var categoiresList = CategoriesCache.CategoryList;

            //Find the Category in the List
            var tempCurrentCategory = categoiresList.SingleOrDefault(item => item.Id == categoryId);

            //Find the parent information
            if (tempCurrentCategory?.ParentCategoryId != null)
            {
                var tempCurrentParentCategory =
                    categoiresList.SingleOrDefault(item => item.Id == tempCurrentCategory.ParentCategoryId);

                parentsList.Add(new CategoryItemViewMoel{Id = tempCurrentParentCategory.Id,Name = tempCurrentParentCategory.Name});

                if (tempCurrentParentCategory.ParentCategoryId.HasValue)
                {
                    //Reload the current parent categories parent
                    ParentCategoryLoad(tempCurrentParentCategory.Id,ref parentsList);
                }
            }

        }

        #endregion



    }



}



