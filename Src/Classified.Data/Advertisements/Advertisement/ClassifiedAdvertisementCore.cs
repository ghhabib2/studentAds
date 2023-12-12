using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using Classified.Data.Base;
using Classified.Domain.Entities;
using Classified.Domain.ViewModels.Advertisment;
using AutoMapper;
using Classified.Domain.ViewModels.Image;

namespace Classified.Data.Advertisements.Advertisement
{
    /// <summary>
    /// Core for Primary registration of Advertisement based on the email address
    /// </summary>
    public class ClassifiedAdvertisementEmailPrimaryCore:RepositoryBase<AdvertisementEmailBasePrimaryRegisterationViewModel, ClassifiedAdvertisement>
    {

    }

    /// <summary>
    /// Classified Advertisement Client Core Class
    /// </summary>
    public class ClassifiedAdvertisementClientCore : RepositoryBase<func_FetchClassifiedAdvertisementsViewModel,
        func_FetchClassifiedAdvertisements>
    {

        /// <summary>
        /// Return to TOP 30 Advertisements for the first page
        /// </summary>
        public IEnumerable<func_FetchClassifiedAdvertisementsViewModel> FetchTop30 => Context.Func_FetchClassifiedAdvertisementsTOP30().AsEnumerable()
            .Select(Mapper.Map<func_FetchClassifiedAdvertisements, func_FetchClassifiedAdvertisementsViewModel>)
            .ToList();

        /// <summary>
        /// Fetch the information of Classified Advertisements based on Root Category
        /// </summary>
        public IEnumerable<func_FetchClassifiedAdvertisementsViewModel> FetchClassifiedAdvertisementsBasedOnRoot => Context.Func_FetchClassifiedAdvertisements().AsEnumerable()
            .Select(Mapper.Map<func_FetchClassifiedAdvertisements, func_FetchClassifiedAdvertisementsViewModel>)
            .ToList();

        /// <summary>
        /// Fetch the information of Classified Advertisements based on their category Id
        /// </summary>
        /// <param name="classifiedCategoryId">Classified Category Id</param>
        /// <returns></returns>
        public IEnumerable<func_FetchClassifiedAdvertisementsViewModel> FetchClassifiedAdvertisementsBasedOnCategory(int classifiedCategoryId)
        {
            return Context.Func_FetchClassifiedAdvertisements(classifiedCategoryId).AsEnumerable()
                .Select(Mapper.Map<func_FetchClassifiedAdvertisements, func_FetchClassifiedAdvertisementsViewModel>)
                .ToList();
        }
    }

    /// <summary>
    /// Classified Advertisement Confirmation Core
    /// </summary>
    public class ClassifiedAdvertisementConfirmationCore : RepositoryBase<AdvertisementConfirmationViewModel,
        ClassifiedAdvertisementConfirmation>
    {
        /// <summary>
        /// Get Advertisement Confirmation info based which includes the information of advertisement itself
        /// </summary>
        /// <param name="emailAddress">Email Address of the mail sender</param>
        /// <param name="token">Token of the Advertisement</param>
        /// <returns></returns>
        public ClassifiedAdvertisementViewModel GetAdvertiement(string emailAddress,string token)
        {
            try
            {
                var tempData = Context.ClassifiedAdvertisementConfirmations.Include(db=>db.ClassifiedAdvertisement)
                    .SingleOrDefault(m => m.EmailAddress == emailAddress && m.ProvidedToken == token)?.ClassifiedAdvertisement;

                //Check if the object is not null and exist
                if (tempData != null)
                {
                    return Mapper.Map<ClassifiedAdvertisement, ClassifiedAdvertisementViewModel>(tempData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }


    /// <summary>
    /// Core for Primary registration of Advertisement based on the email address
    /// </summary>
    public class ClassifiedAdvertisementCore : RepositoryBase<ClassifiedAdvertisementViewModel, ClassifiedAdvertisement>, IClassifiedAdvertisementCore
    {
        public ClassifiedAdvertisementViewModel GetAdsFullInfo(long classifiedAdvertisementId)
        {
            var tempAds = Context.ClassifiedAdvertisements.Include(tbl => tbl.ClassifiedCategory)
                .SingleOrDefault(item=>item.Id==classifiedAdvertisementId);
                

            if (tempAds != null)
            {
                var finalResult= Mapper.Map<ClassifiedAdvertisement, ClassifiedAdvertisementViewModel>(tempAds);

                //Add the Classified Ads Images
                finalResult.ClassifiedImageses = Context.ClassifiedImageses
                    .Where(item => item.ClassifiedAdvertisementId == finalResult.Id).AsEnumerable().Select(Mapper.Map<ClassifiedImages, ImageViewModel>).ToList();

                //Add classified Ads Attributes
                finalResult.Attributes = Context.Func_ClassifiedAdvertisementAttributes(finalResult.Id).AsEnumerable()
                    .Select(Mapper.Map<FunctionClassifiedAdvertisementAttribute, AdvertisementAttributesViewModel>)
                    .ToList();

                //Add the Rejection Lists
                finalResult.RejectComments= Context.AdvertisementRejectComments.Join(Context.Users, reject => reject.RejectedByUser,
                    user => user.Id, (reject, user) => new ClassifiedAdvertisementRejectCommentViewModel
                    {
                        Id = reject.Id,
                        ClassifiedAdvertisementId = reject.ClassifiedAdvertisementId,
                        AdminUserName = user.FirstName + " " + user.LastName,
                        Comment = reject.Comment,
                        RejectedByUser = reject.RejectedByUser
                    }).Where(item => item.ClassifiedAdvertisementId == classifiedAdvertisementId).ToList();

                return finalResult;
            }

            return null;
        }

        public ClassifiedAdvertisementViewModelClient GetAdsFullInfoClient(long classifiedAdvertisementId)
        {
            var tempAds = Context.ClassifiedAdvertisements.Include(tbl => tbl.ClassifiedCategory)
                .SingleOrDefault(item => item.Id == classifiedAdvertisementId);


            if (tempAds != null)
            {
                var finalResult = Mapper.Map<ClassifiedAdvertisement, ClassifiedAdvertisementViewModelClient>(tempAds);

                //Add the Classified Ads Images
                finalResult.ClassifiedImageses = Context.ClassifiedImageses
                    .Where(item => item.ClassifiedAdvertisementId == finalResult.Id).AsEnumerable().Select(Mapper.Map<ClassifiedImages, ImageViewModel>).ToList();

                //Add classified Ads Attributes
                finalResult.Attributes = Context.Func_ClassifiedAdvertisementAttributes(finalResult.Id).AsEnumerable()
                    .Select(Mapper.Map<FunctionClassifiedAdvertisementAttribute, AdvertisementAttributesViewModel>)
                    .ToList();

                //Add the Rejection Lists
                finalResult.RejectComments = Context.AdvertisementRejectComments.Join(Context.Users, reject => reject.RejectedByUser,
                    user => user.Id, (reject, user) => new ClassifiedAdvertisementRejectCommentViewModel
                    {
                        Id = reject.Id,
                        ClassifiedAdvertisementId = reject.ClassifiedAdvertisementId,
                        AdminUserName = user.FirstName + " " + user.LastName,
                        Comment = reject.Comment,
                        RejectedByUser = reject.RejectedByUser
                    }).Where(item => item.ClassifiedAdvertisementId == classifiedAdvertisementId).ToList();

                return finalResult;
            }

            return null;
        }
    }

    /// <summary>
    /// Classified Ads Reject Core
    /// </summary>
    public class ClassifiedAdvertisementRejectCommentCore : RepositoryBase<ClassifiedAdvertisementRejectCommentViewModel
        , ClassifiedAdvertisementRejectComment>
    {
        /// <summary>
        /// Fetch the full information of rejection
        /// </summary>
        /// <param name="classifiedAdsId">Classified Advertisement Id</param>
        /// <returns>List of Rejection for advertisement</returns>
        public IEnumerable<ClassifiedAdvertisementRejectCommentViewModel> GetRejectionFullList(long classifiedAdsId)
        {
            return Context.AdvertisementRejectComments.Join(Context.Users, reject => reject.RejectedByUser,
                user => user.Id, (reject, user) => new ClassifiedAdvertisementRejectCommentViewModel
                {
                    Id = reject.Id,
                    ClassifiedAdvertisementId = reject.ClassifiedAdvertisementId,
                    AdminUserName = user.FirstName + " " + user.LastName,
                    Comment = reject.Comment,
                    RejectedByUser = reject.RejectedByUser
                }).Where(item => item.ClassifiedAdvertisementId == classifiedAdsId).AsEnumerable();
        }
    }

    public interface IClassifiedAdvertisementCore
    {
        /// <summary>
        /// Get the full information of the Advertisement
        /// </summary>
        /// <param name="classifiedAdvertisementId">Classified Advertisement Id</param>
        /// <returns>Classified Advertisement View Model object</returns>
        ClassifiedAdvertisementViewModel GetAdsFullInfo(long classifiedAdvertisementId);
    }

    /// <summary>
    /// Core for modification of advertisement information
    /// </summary>
    public class
        ClassifiedAdvertisementModificationCore : RepositoryBase<ClassifiedAdvertisementModifyViewModel,
            ClassifiedAdvertisement>
    {

        public IEnumerable<AdvertisementAttributesViewModel> PopulateAttributes(
            Expression<Func<ClassifiedAdvertisement, bool>> where)
        {
            //Fetch the information of Advertisement
            var tempAdvertisement = Context.ClassifiedAdvertisements.SingleOrDefault(where);
            if (tempAdvertisement != null)
            {
                //Fetch the information of Category Attributes
                var tempAttributes = Context.ClassifiedCategoryAttributes
                    .Where(item => item.ClassifiedCategoryId == tempAdvertisement.ClassifiedCategoryId).AsEnumerable().Select(Mapper.Map<ClassifiedCategoryAttribute, AdvertisementAttributesViewModel>).ToList();

                //Check if it has any Attributes
                if (tempAttributes.Any())
                {
                    foreach (var attribute in tempAttributes)
                    {
                        //Fetch the Value for options if the control type of attributes control type is drop-down or radio button
                        if (attribute.AttributeControlTypeId == 1 || attribute.AttributeControlTypeId == 2)
                        {
                            attribute.AttributesOptionValues = Context.ClassifiedCategoryAttributeValues
                                .Where(item => item.ClassifiedCategoryAttributeId == attribute.Id).Select(item =>
                                    new AttributesOptionValuesViewModel() {Value = item.AttributeValue});
                        }
                        //Fetch and set the amount of Value for property if there is any
                        attribute.AttributeValue = Context.ClassifiedAdvertisementAttributes
                            .SingleOrDefault(item => item.ClassifiedCategoryAttributeId == attribute.Id && item.ClassifiedAdvertisementId==tempAdvertisement.Id)?.Value;
                    }

                    return tempAttributes;
                }
                else
                {
                    return tempAttributes;
                }
            }
            //Return an empty List
            return new List<AdvertisementAttributesViewModel>();
        }

        /// <summary>
        /// Update the Category of Advertisement
        /// </summary>
        /// <param name="where">Expression for finding the data in database</param>
        /// <param name="newCategoryId">New Category Id to be update in database</param>
        /// <returns></returns>
        public bool UpdateCategory(Expression<Func<ClassifiedAdvertisement, bool>> where,int newCategoryId)
        {
            try
            {
                //Try to find the Data of advertisement in Database
                var tempAds = Context.ClassifiedAdvertisements.FirstOrDefault(where);
                //Check if the data fetched
                if (tempAds!=null)
                {
                    //Remove all the attributes of the Advertisement registered before.

                    //Update Category
                    tempAds.ClassifiedCategoryId = newCategoryId;

                    //Save Changes in Database
                    Context.SaveChanges();


                    return true;
                }
                else
                {
                    throw new ObjectNotFoundException("The requested Advertisement data is not in our database");
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Fetch the full information of rejection
        /// </summary>
        /// <param name="classifiedAdsId">Classified Advertisement Id</param>
        /// <returns>List of Rejection for advertisement</returns>
        public IEnumerable<ClassifiedAdvertisementRejectCommentViewModel> GetRejectionFullList(long classifiedAdsId)
        {
            return Context.AdvertisementRejectComments.Join(Context.Users, reject => reject.RejectedByUser,
                user => user.Id, (reject, user) => new ClassifiedAdvertisementRejectCommentViewModel
                {
                    Id = reject.Id,
                    ClassifiedAdvertisementId = reject.ClassifiedAdvertisementId,
                    AdminUserName = user.FirstName + " " + user.LastName,
                    Comment = reject.Comment,
                    RejectedByUser = reject.RejectedByUser
                }).Where(item => item.ClassifiedAdvertisementId == classifiedAdsId).AsEnumerable();
        }

    }

    /// <summary>
    /// Class for management of Classified Advertisement Attributes
    /// </summary>
    public class
        AdvertisementAttributesCore : RepositoryBase<AdvertisementAttributesValueViewModel,
            ClassifiedAdvertisementAttribute>
    {
        /// <summary>
        /// Update or Insert the information of Attributes for Advertisement
        /// </summary>
        /// <param name="attributes">Attributes as List</param>
        /// <param name="AdsId">Advertisement ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <returns>True if the operation is successful</returns>
        public bool UpdateAdsAttributes(List<AdvertisementAttributesValueViewModel> attributes, long AdsId,
            int categoryId)
        {
            //Fetch the information of Attributes of Category
            //Fetch the information of Category Attributes
            var tempAttributes = Context.ClassifiedCategoryAttributes
                .Where(item => item.ClassifiedCategoryId == categoryId).ToList().Select(Mapper.Map<ClassifiedCategoryAttribute, AdvertisementAttributesViewModel>).ToList();

            //Check if there is a record
            if (tempAttributes.Any())
            {
                
                //Surf the list of Advertisements
                foreach (var attribute in tempAttributes)
                {
                    //Select the record from attributes
                    var attributeToInsert = attributes.FirstOrDefault(item =>
                        item.ClassifiedAdvertisementId == AdsId && item.ClassifiedCategoryAttributeId == attribute.Id);
                    //Fetch and set the amount of Value for property if there is any
                    if (attributeToInsert != null)
                    {
                        var AttributeInDatabase = Context.ClassifiedAdvertisementAttributes
                            .SingleOrDefault(item => item.ClassifiedCategoryAttributeId == attribute.Id && item.ClassifiedAdvertisementId == AdsId);
                        if (AttributeInDatabase != null)
                        {
                            //Update the Record
                            AttributeInDatabase.Value = attributeToInsert.Value;
                        }
                        else
                        {
                            //Insert the Record
                            AttributeInDatabase = new ClassifiedAdvertisementAttribute
                            {
                                ClassifiedAdvertisementId = AdsId,
                                ClassifiedCategoryAttributeId = attributeToInsert.ClassifiedCategoryAttributeId,
                                Value = attributeToInsert.Value
                            };

                            //Add record to Database object
                            Context.ClassifiedAdvertisementAttributes.Add(AttributeInDatabase);
                        }
                    }
                    else
                    {
                        if (attribute.AttributeControlTypeId == 3)
                        {
                            //It is check box. Find and delete the record
                            var AttributeInDatabase = Context.ClassifiedAdvertisementAttributes
                                .SingleOrDefault(item => item.ClassifiedCategoryAttributeId == attribute.Id && item.ClassifiedAdvertisementId == AdsId);
                            if (AttributeInDatabase != null)
                            {
                                //Update the Record
                                Context.ClassifiedAdvertisementAttributes.Remove(AttributeInDatabase);
                            }
                        }
                    }

                }
                //Save Changes into Database
                try
                {
                    Context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }

            return false;
        }

    }
}
