using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Classified.Domain.Entities
{
    public class ClassifiedAdvertisement
    {

        
        /// <summary>
        /// Id of the Advertisement
        /// </summary>
        [Key]
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
        public DateTime? SubmitDate { get; set; }

        /// <summary>
        /// Submit Date
        /// </summary>
        [Display(Name = "Approvement Date")]
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [Display(Name = "Created On UTC")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Update date of the Advertisement
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

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
        /// Parent Classified Category Id
        /// </summary>
        [Required(ErrorMessage = "You have to specify the category of the advertisement you are going to submit.")]
        public int ClassifiedCategoryId { get; set; }

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
        /// Flag controlling if the advertisement is email based
        /// </summary>
        public bool IsEmailBase { get; set; }

        /// <summary>
        /// Parent Classified Category Object
        /// </summary>
        public ClassifiedCategory ClassifiedCategory { get; set; }

        /// <summary>
        /// Reviewed By User
        /// </summary>
        [StringLength(128)]
        public string ReviewedByUser { get; set; }

        /// <summary>
        /// Review By User who reviewed this advertisement recently
        /// </summary>
        public bool HasReviewPriority { get; set; }

        //--------------------- Not included in new version ----------------------------------

        //public virtual ICollection<ClassifiedPicture> ClassifiedPicture { get; set; }
        //public virtual ICollection<ClassifiedComment> ClassifiedComments { get; set; }
        //public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<AttributeValue> AttributeValues { get; set; }

        //public int LocationId { get; set; }
        //public virtual ICollection<Location> Locations { get; set; }

    }

    /// <summary>
    /// Class of the Classified Advertisement Rejection Comments
    /// </summary>
    public class ClassifiedAdvertisementRejectComment
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
        /// Id of the related Classified Advertisement
        /// </summary>
        public long ClassifiedAdvertisementId { get; set; }

        /// <summary>
        /// Rejected By User
        /// </summary>
        [StringLength(128)]
        public string RejectedByUser { get; set; }

        /// <summary>
        /// Parent Classified Advertisement Object
        /// </summary>
        public ClassifiedAdvertisement ClassifiedAdvertisement { get; set; }
        
    }

    /// <summary>
    /// Classified Advertisement Confirmation
    /// </summary>
    public class ClassifiedAdvertisementConfirmation
    {
        /// <summary>
        /// The Primary Key of the Confirmation Table
        /// </summary>
        [Key]
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
        /// Parent Classified Advertisement Object
        /// </summary>
        public ClassifiedAdvertisement ClassifiedAdvertisement { get; set; }
    }

    public class ClassifiedComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string Email { get; set; }

        public string Comment { get; set; }


        public DateTime CreateDate { get; set; }

        public long ClassifiedAdvertisementId { get; set; }

        public ClassifiedAdvertisement ClassifiedAd { get; set; }

    }

    public class func_FetchClassifiedAdvertisements
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

    }

}