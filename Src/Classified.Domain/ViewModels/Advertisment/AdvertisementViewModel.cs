using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Web.Mvc;
using Classified.Domain.CustomValidationControl;
using Classified.Domain.Entities;
using Classified.Domain.ViewModels.Base;
using Classified.Domain.ViewModels.Image;

namespace Classified.Domain.ViewModels.Advertisment
{
    public enum AdvertisementModificationFlag
    {
        UpdateCategory = 1,
        UpdatePrimaryData = 2,
        UpdateAttributes = 3,
        UpdateImages = 4,
        UpdateDescription = 5,
        UpdateSeo = 6
    }


    public class ClassifiedAdvertisementListClientViewModel : ClientViewModel
    {
        public ClassifiedAdvertisementListClientViewModel()
        {
            CategoryList = new List<CategoryItemClientViewMoel>();
            AdsList=new List<func_FetchClassifiedAdvertisementsViewModel>();
            SelectedClassifiedCategoryCategoryId=String.Empty;
            SelectedClassifiedCategoryName = string.Empty;
            RecordCount = 0;
        }

        /// <summary>
        /// Categories List
        /// </summary>
        public IEnumerable<CategoryItemClientViewMoel> CategoryList { get; set; }

        /// <summary>
        /// List of advertisements
        /// </summary>
        public IEnumerable<func_FetchClassifiedAdvertisementsViewModel> AdsList { get; set; }

        /// <summary>
        /// Find out the number of Records to be used in the page
        /// </summary>
        public long RecordCount { get; set; }

        /// <summary>
        /// Selected Classified Category Id
        /// </summary>
        [Display(Name = "Categories")]
        public string SelectedClassifiedCategoryCategoryId { get; set; }

        /// <summary>
        /// Selected Category Name
        /// </summary>
        public  string SelectedClassifiedCategoryName { get; set; }

    }


    /// <summary>
    /// Model View for Primary Registration of an Advertisement using email address
    /// </summary>
    public class AdvertisementEmailBasePrimaryRegisterationViewModel
    {
        /// <summary>
        /// Default values of the View Model
        /// </summary>
        public AdvertisementEmailBasePrimaryRegisterationViewModel()
        {
            Id = -1;
            Title = string.Empty;
            ShortDescription = string.Empty;
            Address = string.Empty;
            SubmitDate = DateTime.UtcNow;
            EmailAddress = string.Empty;
            IsEmailConfirmed = false;
            ClassifiedCategoryId = null;
            Categories = new List<CategoryItemClientViewMoel>();
            IsReSubmitted = false;
        }

        /// <summary>
        /// Id of the Advertisement
        /// </summary>
        [Key]
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        /// <summary>
        /// Check whether the form is Re-Submitted or not
        /// </summary>
        public bool IsReSubmitted { get; set; }

        /// <summary>
        /// Determine if the advertisement is email based or not
        /// </summary>
        public bool IsEmailBase { get; set; }

        /// <summary>
        /// Title of the Advertisement
        /// </summary>
        [Required(ErrorMessage = "Entering advertisement title is essential for this operation!!")]
        [Display(Name = "Advertisement Title")]
        public string Title { get; set; }

        /// <summary>
        /// Short Description  of Advertisement
        /// </summary>
        [Display(Name = " Advertisement Short Description ")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Address of Advertisement Owner
        /// </summary>
        [Required(ErrorMessage = "Entering address is essential for this operation!!")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Submit Date
        /// </summary>
        [Display(Name = "Submit Date on UTC")]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Submit Date
        /// </summary>
        [Display(Name = "Creation Date on UTC")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Updated Date
        /// </summary>
        [Display(Name = "Updated On UTC")]
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Advertisement  Owner's Email Address
        /// </summary>
        [Required(ErrorMessage = "Your email address is required for this operation!!")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Email Address Confirmation Flag.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Parent Classified Category Id
        /// </summary>
        [Required(ErrorMessage = "You have to specify the category of the advertisement you are going to submit.")]
        [CategorySelectionValidationSubmitByEmailAddress]
        [Display(Name = "Advertisement Category")]
        public int? ClassifiedCategoryId { get; set; }

        /// <summary>
        /// Parent Classified Category Object
        /// </summary>
        public List<CategoryItemClientViewMoel> Categories { get; set; }
    }


    /// <summary>
    /// View Model for Classified Ads 
    /// </summary>
    public class ClassifiedAdvertisementViewModelClient:ClientViewModel
    {

        /// <summary>
        /// Id of the Advertisement
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }
        /// <summary>
        /// Title of the Advertisement
        /// </summary>
        [Required]
        [Display(Name = "Advertisement Title")]
        public string Title { get; set; }

        /// <summary>
        /// Meta Description of the Advertisement
        /// </summary>
        [Display(Name = "Advertisement Meta Description ")]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Title of the Advertisement
        /// </summary>
        [Display(Name = "Advertisement Meta Title ")]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Meta Keyword of Advertisement
        /// </summary>
        [Display(Name = "Advertisement Meta KeyWord ")]
        public string MetaKeyWord { get; set; }

        /// <summary>
        /// Short Description  of Advertisement
        /// </summary>
        [Display(Name = " Advertisement Short Description ")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Main Description of Advertisement
        /// </summary>
        [Display(Name = "Advertisement Description ")]
        public string Description { get; set; }

        /// <summary>
        /// Address of Advertisement Owner
        /// </summary>
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Phone number of Advertisement Number
        /// </summary>
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Is the Advertisement Active
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Submit Date
        /// </summary>
        [Display(Name = "Submit Date")]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        [Display(Name = "Final Modification Date")]
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [Display(Name = "Created On UTC")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Advertisement  Owner's Email Address
        /// </summary>
        [Required(ErrorMessage = "EmailID is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Email Address Confirmation Flag.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Check if the Advertisement Deleted (Trash Flag)
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Price of the Advertisement 
        /// </summary>
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Featured Advertisement Flag
        /// </summary>
        [Display(Name = "Is Featured")]
        public bool IsEnhanced { get; set; }

        /// <summary>
        /// Flag for controlling if this is a preview
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        /// Flag Controlling if this is an email base Advertisement
        /// </summary>
        public bool IsEmailBase { get; set; }

        /// <summary>
        /// Parent Classified Category Id
        /// </summary>
        [Required(ErrorMessage = "You have to specify the category of the advertisement you are going to submit.")]
        public int ClassifiedCategoryId { get; set; }

        /// <summary>
        /// Parent Classified Category Object
        /// </summary>
        public CategoryViewMoel ClassifiedCategory { get; set; }

        /// <summary>
        /// Flag controlling if the advertisement submitted before
        /// </summary>
        public bool IsSubmitted { get; set; }

        /// <summary>
        /// Flag Controlling if the advertisement approved.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Flag Controlling if the advertisement rejected
        /// </summary>
        public bool IsRejected { get; set; }


        /// <summary>
        /// Approvement Date
        /// </summary>
        [Display(Name = "Approvement Date")]
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// Advertisement Attributes
        /// </summary>
        public List<AdvertisementAttributesViewModel> Attributes { get; set; }

        /// <summary>
        /// Classified Images
        /// </summary>s
        public List<ImageViewModel> ClassifiedImageses { get; set; }

        public List<ClassifiedAdvertisementRejectCommentViewModel> RejectComments { get; set; }

        /// <summary>
        /// ReviewedByUser
        /// </summary>
        [Display(Name = "Reviewed By")]
        public string ReviewedByUser { get; set; }

        /// <summary>
        /// Review By User who reviewed this advertisement recently
        /// </summary>
        public bool HasReviewPriority { get; set; }

    }

    /// <summary>
    /// View Model for Classified Ads 
    /// </summary>
    public class ClassifiedAdvertisementViewModel
    {

        /// <summary>
        /// Id of the Advertisement
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }
        /// <summary>
        /// Title of the Advertisement
        /// </summary>
        [Required]
        [Display(Name = "Advertisement Title")]
        public string Title { get; set; }

        /// <summary>
        /// Meta Description of the Advertisement
        /// </summary>
        [Display(Name = "Advertisement Meta Description ")]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Title of the Advertisement
        /// </summary>
        [Display(Name = "Advertisement Meta Title ")]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Meta Keyword of Advertisement
        /// </summary>
        [Display(Name = "Advertisement Meta KeyWord ")]
        public string MetaKeyWord { get; set; }

        /// <summary>
        /// Short Description  of Advertisement
        /// </summary>
        [Display(Name = " Advertisement Short Description ")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Main Description of Advertisement
        /// </summary>
        [Display(Name = "Advertisement Description ")]
        public string Description { get; set; }

        /// <summary>
        /// Address of Advertisement Owner
        /// </summary>
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Phone number of Advertisement Number
        /// </summary>
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Is the Advertisement Active
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Submit Date
        /// </summary>
        [Display(Name = "Submit Date")]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        [Display(Name = "Final Modification Date")]
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [Display(Name = "Created On UTC")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Advertisement  Owner's Email Address
        /// </summary>
        [Required(ErrorMessage = "EmailID is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Email Address Confirmation Flag.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Check if the Advertisement Deleted (Trash Flag)
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Price of the Advertisement 
        /// </summary>
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Featured Advertisement Flag
        /// </summary>
        [Display(Name = "Is Featured")]
        public bool IsEnhanced { get; set; }

        /// <summary>
        /// Flag for controlling if this is a preview
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        /// Flag Controlling if this is an email base Advertisement
        /// </summary>
        public bool IsEmailBase { get; set; }

        /// <summary>
        /// Parent Classified Category Id
        /// </summary>
        [Required(ErrorMessage = "You have to specify the category of the advertisement you are going to submit.")]
        public int ClassifiedCategoryId { get; set; }

        /// <summary>
        /// Parent Classified Category Object
        /// </summary>
        public CategoryViewMoel ClassifiedCategory { get; set; }

        /// <summary>
        /// Flag controlling if the advertisement submitted before
        /// </summary>
        public bool IsSubmitted { get; set; }

        /// <summary>
        /// Flag Controlling if the advertisement approved.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Flag Controlling if the advertisement rejected
        /// </summary>
        public bool IsRejected { get; set; }


        /// <summary>
        /// Approvement Date
        /// </summary>
        [Display(Name = "Approvement Date")]
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// Advertisement Attributes
        /// </summary>
        public List<AdvertisementAttributesViewModel> Attributes { get; set; }

        /// <summary>
        /// Classified Images
        /// </summary>s
        public List<ImageViewModel> ClassifiedImageses { get; set; }

        public List<ClassifiedAdvertisementRejectCommentViewModel> RejectComments { get; set; }

        /// <summary>
        /// ReviewedByUser
        /// </summary>
        [Display(Name = "Reviewed By")]
        public string ReviewedByUser { get; set; }

        /// <summary>
        /// Review By User who reviewed this advertisement recently
        /// </summary>
        public bool HasReviewPriority { get; set; }

    }

    /// <summary>
    /// Class of the Classified Advertisement Rejection Comments
    /// </summary>
    public class ClassifiedAdvertisementRejectCommentViewModel
    {
        /// <summary>
        /// Id of the Comment
        /// </summary>
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Comment of the rejection at the moment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Rejected By User
        /// </summary>
        [StringLength(128)]
        public string RejectedByUser { get; set; }

        /// <summary>
        /// Admin User Name who rejected this advertisement
        /// </summary>
        public string AdminUserName { get; set; }

        /// <summary>
        /// Classified Advertisement Id
        /// </summary>
        public long ClassifiedAdvertisementId { get; set; }

    }

    /// <summary>
    /// Advertisement Confirmation View Model
    /// </summary>
    public class AdvertisementConfirmationViewModel
    {
        public AdvertisementConfirmationViewModel()
        {
            Id = -1;
            ProvidedToken = string.Empty;
            EmailAddress = string.Empty;
            ClassifiedAdvertisementId = -1;
        }

        /// <summary>
        /// The Primary Key of the Confirmation Table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Provided Token
        /// </summary>
        public string ProvidedToken { get; set; }

        /// <summary>
        /// Email address of Advertisement Provider
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Id of the related Classified Advertisement
        /// </summary>
        public long ClassifiedAdvertisementId { get; set; }

        /// <summary>
        /// Classified Advertisement View Model
        /// </summary>
        public ClassifiedAdvertisementViewModel ClassifiedAdvertisement { get; set; }
    }

    /// <summary>
    /// Email Base Ads Confirmation View, View Model
    /// </summary>
    public class EmailBaseAdsConfirmationViewModel
    {
        /// <summary>
        /// Email Address of the customer
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Link to Access the advertisement modification page
        /// </summary>
        public string EmailLink { get; set; }

    }

    /// <summary>
    /// Classified Advertisement Modify View Model
    /// </summary>
    public class ClassifiedAdvertisementModifyViewModel
    {
        /// <summary>
        /// Default Values for Advertisement Modify View Model
        /// </summary>
        public ClassifiedAdvertisementModifyViewModel()
        {
            Id = -1;
            Title = string.Empty;
            MetaTitle = string.Empty;
            MetaDescription = string.Empty;
            MetaKeyWord = string.Empty;
            ShortDescription = string.Empty;
            Description = string.Empty;
            Address = string.Empty;
            PhoneNumber = string.Empty;
            IsActive = false;
            EmailAddress = string.Empty;
            Deleted = false;
            SubmitDate = DateTime.UtcNow;
            CreatedOnUtc = DateTime.UtcNow;
            AdvertisementModificationFlag = AdvertisementModificationFlag.UpdateCategory;
            AdvertisementAttributes = new List<AdvertisementAttributesViewModel>();
            ClassifiedCategoryId = -1;
            ClassifiedCategory = null;
            ClassifiedCategoryList = new List<CategoryItemClientViewMoel>();
            IsEmailConfirmed = false;
            IsEnhanced = false;
            IsFillData = false;
            Price = -1;
            Images = new List<ImageViewModel>();
        }

        /// <summary>
        /// Id of the Advertisement
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        /// <summary>
        /// Title of the Advertisement
        /// </summary>
        [Required]
        [Display(Name = "Advertisement Title")]
        public string Title { get; set; }

        /// <summary>
        /// Meta Description of the Category (SEO)
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Title of the Category (SEO)
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Meta keywords of the Category (SEO)
        /// </summary>
        public string MetaKeyWord { get; set; }

        /// <summary>
        /// Short Description  of Advertisement
        /// </summary>
        [Display(Name = " Advertisement Short Description ")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Main Description of Advertisement
        /// </summary>
        [Display(Name = "Advertisement Description ")]
        public string Description { get; set; }

        /// <summary>
        /// Address of Advertisement Owner
        /// </summary>
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Phone number of Advertisement Number
        /// </summary>
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^([\+]?(?:00)?[0-9]{1,3}[\s.-]?[0-9]{1,12})([\s.-]?[0-9]{1,4}?)$", ErrorMessage = "The phone number should include a '+' followed by a group of number. Here is a sample (+60123456789)")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Is the Advertisement Active
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Submit Date
        /// </summary>
        [Display(Name = "Submit Date")]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        [Display(Name = "Final Modification Date")]
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Approvement Date
        /// </summary>
        [Display(Name = "Final Approvement Date")]

        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [Display(Name = "Created On UTC")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Advertisement  Owner's Email Address
        /// </summary>
        [Required(ErrorMessage = "EmailID is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Email Address Confirmation Flag.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Flag controlling if the advertisement submitted before
        /// </summary>
        public bool IsSubmitted { get; set; }

        /// <summary>
        /// Flag Controlling if the advertisement approved.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Flag Controlling if the advertisement rejected
        /// </summary>
        public bool IsRejected { get; set; }


        /// <summary>
        /// Check if the Advertisement Deleted (Trash Flag)
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Price of the Advertisement 
        /// </summary>
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Featured Advertisement Flag
        /// </summary>
        [Display(Name = "Is Featured")]
        public bool IsEnhanced { get; set; }

        /// <summary>
        /// Parent Classified Category Id
        /// </summary>
        [Required(ErrorMessage = "You have to specify the category of the advertisement you are going to submit.")]
        [Display(Name = "Advertisement Category")]
        [CategorySelectionValidationAdvertisementModification]
        public int? ClassifiedCategoryId { get; set; }

        /// <summary>
        /// Parent Classified Category Object
        /// </summary>
        public CategoryViewMoel ClassifiedCategory { get; set; }

        /// <summary>
        /// List of categories to be displayed
        /// </summary>
        public IEnumerable<CategoryItemClientViewMoel> ClassifiedCategoryList { get; set; }

        /// <summary>
        /// Flag for finding out the current state of the modification
        /// </summary>
        public AdvertisementModificationFlag AdvertisementModificationFlag { get; set; }

        /// <summary>
        /// Flag to find out if the system should Fill the form data again?
        /// </summary>
        public bool IsFillData { get; set; }

        /// <summary>
        /// Advertisement Attributes
        /// </summary>
        public List<AdvertisementAttributesViewModel> AdvertisementAttributes { get; set; }

        public List<ImageViewModel> Images { get; set; }

        public List<ClassifiedAdvertisementRejectCommentViewModel> RejectionReasonsLIst { get; set; }

    }

    /// <summary>
    /// View Model for Attributes Options
    /// </summary>
    public class AttributesOptionValuesViewModel
    {
        /// <summary>
        /// Default Values of the Options Values View Model
        /// </summary>
        public AttributesOptionValuesViewModel()
        {
            Value = string.Empty;
        }
        public string Value { get; set; }
    }

    /// <summary>
    /// Attributes for Advertisement
    /// </summary>
    public class AdvertisementAttributesViewModel
    {
        /// <summary>
        /// Default Values for Advertisement Attributes
        /// </summary>
        public AdvertisementAttributesViewModel()
        {
            Id = -1;
            AttributeLabel = string.Empty;
            AttributeName = string.Empty;
            AttributeControlTypeId = -1;
            AttributeValue = string.Empty;
            AttributesOptionValues = new List<AttributesOptionValuesViewModel>();
        }

        /// <summary>
        /// Id of the Attribute
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Attribute Label
        /// </summary>
        public string AttributeLabel { get; set; }
        /// <summary>
        /// Attribute Name
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// Attribute Control Type Id
        /// </summary>
        public int AttributeControlTypeId { get; set; }
        /// <summary>
        /// Attribute Value
        /// </summary>
        public string AttributeValue { get; set; }

        /// <summary>
        /// List of Attributes Control Options if there is any
        /// </summary>
        public IEnumerable<AttributesOptionValuesViewModel> AttributesOptionValues { get; set; }
    }

    /// <summary>
    /// View Model for Attributes Value View Model
    /// </summary>
    public class AdvertisementAttributesValueViewModel
    {
        public AdvertisementAttributesValueViewModel()
        {
            Id = -1;
            Value = string.Empty;
            ClassifiedAdvertisementId = -1;
            ClassifiedCategoryAttributeId = -1;
        }

        /// <summary>
        /// Id of the Attribute Value
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Value of the Attribute
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Parent Classified Category Attribute Id
        /// </summary>
        public int ClassifiedCategoryAttributeId { get; set; }
        /// <summary>
        /// Parent Classified Advertisement Id
        /// </summary>
        public long ClassifiedAdvertisementId { get; set; }
    }

    /// <summary>
    /// View Model for Advertisement Description
    /// </summary>
    public class AdvertismentDescriptionViewModel
    {
        /// <summary>
        /// Main Description of Advertisement
        /// </summary>
        [Display(Name = "Advertisement Description ")]
        [AllowHtml]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Id of the Advertisement
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Email Address of the Advertisement Owner
        /// </summary>
        public string EmailAddress { get; set; }

    }

    /// <summary>
    /// SEO View Model for Advertisement
    /// </summary>
    public class AdvertisementSeoViewModel
    {
        /// <summary>
        /// Id of the Advertisement
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Email Address of the Advertisement Owner
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Meta Description of the Category (SEO)
        /// </summary>
        //[StringLength(300, MinimumLength = 50, ErrorMessage = "Meta Description must have at least 50 characters and at most 300 characters.")]
        [Display(Name = "Meta Descriptions ")]
        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Title of the Category (SEO)
        /// </summary>
        //[StringLength(60, MinimumLength = 50, ErrorMessage = "Meta Title must have between 50 to 60 characters.")]
        [Display(Name = "Meta Title ")]
        [DataType(DataType.Text)]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Meta keywords of the Category (SEO)
        /// </summary>
        //[StringLength(120, ErrorMessage = "Meta keywords cannot have more than 120 characters")]
        [Display(Name = "Meta KeyWord ")]
        [DataType(DataType.Text)]
        public string MetaKeyWord { get; set; }

    }

    /// <summary>
    /// View Model for Classified Advertisement Management
    /// </summary>
    public class ClassifiedAdvertismentManagementViewModel
    {
        /// <summary>
        /// Default Value for Advertisement View Model
        /// </summary>
        public ClassifiedAdvertismentManagementViewModel()
        {
            AdsList = new List<ClassifiedAdvertisementViewModel>();
            ClassifiedAds = null;
            Id = -1;
        }
        /// <summary>
        /// List of Advertisements to be managed
        /// </summary>
        public IEnumerable<ClassifiedAdvertisementViewModel> AdsList { get; set; }

        /// <summary>
        /// Classified Advertisement that its information is going to be managed
        /// </summary>
        public ClassifiedAdvertisementViewModel ClassifiedAds { get; set; }

        

        /// <summary>
        /// Target Classified Advertisement Id
        /// </summary>
        public long Id { get; set; }
    }

    public class func_FetchClassifiedAdvertisementsViewModel
    {

        /// <summary>
        /// Advertisement Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Title of the Advertisement
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Short Description of Advertisement
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Date that the Advertisement updated or promoted
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Price of Advertisement
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Check if this is a kind of Enhanced Advertisement
        /// </summary>
        public bool IsEnhanced { get; set; }

        /// <summary>
        /// Image Name
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Classified Category Id
        /// </summary>
        public int ClassifiedCategoryId { get; set; }

        /// <summary>
        /// Classified Category Name
        /// </summary>
        public string ClassifiedCategoryName { get; set; }

        /// <summary>
        /// URL of the Category
        /// </summary>
        public string CategoryUrl { get; set; }

        /// <summary>
        /// URL of the Advertisement
        /// </summary>
        public string AdsUrl { get; set; }

        /// <summary>
        /// Short Date string of the Date
        /// </summary>
        public string DateString { get; set; }

        /// <summary>
        /// String of the Price
        /// </summary>
        public string PriceString { get; set; }

    }

}
