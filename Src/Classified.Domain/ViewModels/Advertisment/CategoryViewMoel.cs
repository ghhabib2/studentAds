using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Classified.Domain.Entities;

namespace Classified.Domain.ViewModels.Advertisment
{

    /// <summary>
    /// View Model for working with Categories
    /// </summary>
    public class CategoryViewMoel
    {
        /// <summary>
        /// initial state of the Category View Model
        /// </summary>
        public CategoryViewMoel()
        {
            Id = -1;
            MetaDescription = string.Empty;
            MetaKeyWord = string.Empty;
            MetaTitle = string.Empty;
            Description = string.Empty;
            Name = string.Empty;
            IsActive = false;
            Deleted = false;
            DisplayOrder = 0;
            Published = false;
            ShowOnHomePage = false;
            IsPriceShown = false;
            CreatedOnUtc = DateTime.UtcNow;
            UpdatedOnUtc = DateTime.UtcNow;
            ParentCategoryId = null;
            ParentCategory = null;
        }

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
        [StringLength(300, MinimumLength = 50, ErrorMessage = "Meta Description must have at least 50 characters and at most 300 characters.")]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Title of the Category (SEO)
        /// </summary>
        [Display(Name = "Meta Title ")]
        [DataType(DataType.Text)]
        [StringLength(60, MinimumLength = 50, ErrorMessage = "Meta Title must have between 50 to 60 characters.")]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Meta keywords of the Category (SEO)
        /// </summary>
        [Display(Name = "Meta KeyWord ")]
        [DataType(DataType.Text)]
        [StringLength(120, ErrorMessage = "Meta keywords cannot have more than 120 characters")]
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
        [Required(ErrorMessage = "Category name is required for this operation.")]
        [Display(Name = "Category Name")]
        [DataType(DataType.Text)]
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Control if the Category is Active
        /// </summary>
        [Display(Name = "Is this category active?")]
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Check if the Category is deleted
        /// </summary>
        [Display(Name = "Is this category deleted?")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Display order of the Category
        /// </summary>
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Check if the Category is published
        /// </summary>
        [Display(Name = "Publish this category?")]
        public bool Published { get; set; }

        /// <summary>
        /// Check if the system should display the Category in home page.
        /// </summary>
        [Display(Name = "Do you want me to show this category in home page?")]
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Check if the price should be displayed for this Category.
        /// </summary>
        [Display(Name = "Show prices for this category?")]
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
        public CategoryViewMoel ParentCategory { get; set; }
    }

    /// <summary>
    /// Category Item Model that will be used for generation of Hierarchy List
    /// </summary>
    public class CategoryItemViewMoel
    {
        /// <summary>
        /// Primary Key of the Category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the Category
        /// </summary>
        public string Name { get; set; }
    }


    /// <summary>
    /// Category Item Model that will be used for generation of Hierarchy List for Client Side
    /// </summary>
    public class CategoryItemClientViewMoel
    {
        public CategoryItemClientViewMoel()
        {
            Id = string.Empty;
            Name = string.Empty;
            Url = string.Empty;
            ChildCategories=new List<CategoryItemClientViewMoel>();
        }


        /// <summary>
        /// URL of the Category
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Primary Key of the Category
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the Category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Child Categories for the target Category
        /// </summary>
        public List<CategoryItemClientViewMoel> ChildCategories { get; set; }
    }

    /// <summary>
    /// Use to display the Category Management Content in the View
    /// </summary>
    public class CategoryManagmentViewMoel
    {

        public CategoryManagmentViewMoel()
        {
            CategoryType = 0;
            CategoryId = -1;
        }

        /// <summary>
        /// List of the Categories in Hierarchy mode
        /// </summary>
        public IEnumerable<CategoryItemViewMoel> HierarchyCategoryList { get; set; }

        /// <summary>
        /// Category Id that will be used for Drop Down List
        /// </summary>
        [Display(Name = "Categories List")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Category Type to Display
        /// </summary>
        public byte CategoryType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Categories type to display")]
        public IEnumerable<CategoryType> CategoryTypes { get; set; }

        /// <summary>
        /// Target Categories that must be displayed in the Table
        /// </summary>
        public IEnumerable<CategoryViewMoel> TargetCategories { get; set; }
    }

    /// <summary>
    /// Register Category View Model
    /// </summary>
    public class CategoryRegisterViewMoel
    {


        /// <summary>
        /// Category View Model
        /// </summary>
        public CategoryViewMoel Category { get; set; }

        /// <summary>
        /// List of categories in Hierarchy Mode
        /// </summary>
        public IEnumerable<CategoryItemViewMoel> CateogryList { get; set; }

        /// <summary>
        /// Parent Category Id that will be used for Drop Down List
        /// </summary>
        [Display(Name = "Parent Category")]
        public int ParentCategoryId { get; set; }
    }

    public class CategoryType
    {
        /// <summary>
        /// Id of the Category
        /// </summary>
        public byte Id { get; set; }

        //Name of the Category
        public string Name { get; set; }
    }

}
