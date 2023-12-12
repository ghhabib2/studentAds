using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Mvc;

namespace Classified.Domain.Entities
{
    /// <summary>
    /// Class defines the Model of the category
    /// </summary>
    public class ClassifiedCategory
    {
        /// <summary>
        /// Primary Key of the Category
        /// </summary>
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Meta Description of the Category (SEO)
        /// </summary>
        [Display(Name = "Meta Descriptions ")]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Title of the Category (SEO)
        /// </summary>
        [Display(Name = "Meta Title ")]
        [DataType(DataType.Text)]
        [StringLength(60)]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Meta keywords of the Category (SEO)
        /// </summary>
        [Display(Name = "Meta KeyWord ")]
        [DataType(DataType.Text)]
        [StringLength(120)]
        public string MetaKeyWord { get; set; }

        /// <summary>
        /// Description of the Category
        /// </summary>
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /// <summary>
        /// Name of the Category
        /// </summary>
        [Required]
        [Display(Name = "Category Name")]
        [DataType(DataType.Text)]
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Control if the Category is Active
        /// </summary>
        [Display(Name = "Is Active")]
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Check if the Category is deleted
        /// </summary>
        [Display(Name = "Deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Display order of the Category
        /// </summary>
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Check if the Category is published
        /// </summary>
        [Display(Name = "Published")]
        public bool Published { get; set; }

        /// <summary>
        /// Check if the system should display the Category in home page.
        /// </summary>
        [Display(Name = "Show on home page")]
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Check if the price should be displayed for this Category.
        /// </summary>
        [Display(Name = "Show price")]
        public bool IsPriceShown { get; set; }

        /// <summary>
        /// Creation Time for the Category
        /// </summary>
        [Display(Name = "Creation Time")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Latest Time and Date that this Category Updated
        /// </summary>
        [Display(Name = "Update Time")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Parent Category's Id
        /// </summary>
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Parent Category's Object
        /// </summary>
        [Display(Name = "Parent Category")]
        public ClassifiedCategory ParentCategory { get; set; }

    }

}