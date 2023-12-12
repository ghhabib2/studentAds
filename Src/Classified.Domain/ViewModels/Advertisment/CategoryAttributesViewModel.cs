using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Classified.Domain.ViewModels.Advertisment
{

    /// <summary>
    /// Enum for Control Type
    /// </summary>
    public enum AttributeControlType
    {
        /// <summary>
        /// Drop-down list
        /// </summary>
        DropdownList = 1,
        /// <summary>
        /// Radio list
        /// </summary>
        RadioList = 2,
        /// <summary>
        /// Check-boxes
        /// </summary>
        Checkboxes = 3,
        /// <summary>
        /// TextBox
        /// </summary>
        TextBox = 4,
        /// <summary>
        /// Multi-line text-box
        /// </summary>
        MultilineTextbox = 5,
        /// <summary>
        /// Date picker
        /// </summary>
        Datepicker = 6,

    }

    /// <summary>
    /// Attribute Search By Enum
    /// </summary>
    public enum AttributeSearchBy
    {
        MultipleChoice = 1,
        Range = 2
    }

    /// <summary>
    /// View Model for CAtegory Attributes
    /// </summary>
    public class CategoryAttributesViewModel
    {
        /// <summary>
        /// Default Values for Category Attributes View Model
        /// </summary>
        public CategoryAttributesViewModel()
        {
            Id = -1;
            AttributeLabel = string.Empty;
            AttributeName = string.Empty;
            AttributeSearchBy = 1;
            AttributeControlTypeId = 1;
            ClassifiedCategoryId = null;
            ClassifiedCategory = null;
        }

        /// <summary>
        /// Primary key of each Attribute
        /// </summary>
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Attribute Label
        /// </summary>
        [Required(ErrorMessage = "Entering attribute label is essential for this operation!!")]
        [Display(Name = "Attribute Label")]
        public string AttributeLabel { get; set; }

        /// <summary>
        /// Attribute Name
        /// </summary>
        [Required(ErrorMessage = "Entering attribute name is essential for this operation!!")]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [Display(Name = "Attribute Name")]
        public string AttributeName { get; set; }


        /// <summary>
        /// Attribute Search By
        /// </summary>
        [Display(Name = "Search By")]
        public int AttributeSearchBy { get; set; }

        /// <summary>
        /// Attribute Control Type
        /// </summary>
        [Display(Name = "Control Type")]
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// Attribute's Category Id
        /// </summary>
        public int? ClassifiedCategoryId { get; set; }

        /// <summary>
        /// Attribute's Category View Model
        /// </summary>
        public CategoryViewMoel ClassifiedCategory { get; set; }
    }

    /// <summary>
    /// View Model for displaying information in CategoryyAttrributes Management Page
    /// </summary>
    public class CategoryAttributesManagmentViewModel
    {
        /// <summary>
        /// Default values for Category Attributes View Model
        /// </summary>
        public CategoryAttributesManagmentViewModel()
        {
            CategoryId = -1;
            HierarchyCategoryList=new List<CategoryItemViewMoel>();
            CategoryAttributes=new List<CategoryAttributesViewModel>();
            CategoryAttribut=new CategoryAttributesViewModel();
            CategoryAttributesControlType = PopulateControlTypes();
            CategoryAttributesSearchByList = PopulateSearchByes();
            CategoryAttributeId = -1;
        }

        /// <summary>
        /// List of the Categories in Hierarchy mode
        /// </summary>
        public IEnumerable<CategoryItemViewMoel> HierarchyCategoryList { get; set; }

        /// <summary>
        /// Target Category Id
        /// </summary>
        [Display(Name = "Target Category")]
        public int CategoryId { get; set; }

        /// <summary>
        /// List of Attributes related to the Category
        /// </summary>
        public IEnumerable<CategoryAttributesViewModel> CategoryAttributes { get; set; }

        /// <summary>
        /// The Category Attribute Object that will be used for information exchange with database
        /// </summary>
        public CategoryAttributesViewModel CategoryAttribut { get; set; }

        /// <summary>
        /// The Category Attribute Id
        /// </summary>
        public int CategoryAttributeId { get; set; }

        /// <summary>
        /// List for displaying the information of different Attributes Control Types
        /// </summary>
        public IEnumerable<CategoryAttributesControlTypeViewModel> CategoryAttributesControlType { get; set; }

        /// <summary>
        /// List for displaying the information of different search methods for the attribute
        /// </summary>
        public IEnumerable<CategoryAttributesSearchByViewModel> CategoryAttributesSearchByList { get; set; }

        

        /// <summary>
        /// Internal method used for populating the Control Types
        /// </summary>
        /// <returns></returns>
        protected internal List<CategoryAttributesControlTypeViewModel> PopulateControlTypes()
        {
            var tempControlTypesList = new List<CategoryAttributesControlTypeViewModel>();

            tempControlTypesList.Add(new CategoryAttributesControlTypeViewModel { ControlTypeId = 1, ControlTypeName = "Drop-down List" });
            tempControlTypesList.Add(new CategoryAttributesControlTypeViewModel { ControlTypeId = 2, ControlTypeName = "Radio List" });
            tempControlTypesList.Add(new CategoryAttributesControlTypeViewModel { ControlTypeId = 3, ControlTypeName = "Check boxes" });
            tempControlTypesList.Add(new CategoryAttributesControlTypeViewModel { ControlTypeId = 4, ControlTypeName = "Text Box" });
            tempControlTypesList.Add(new CategoryAttributesControlTypeViewModel { ControlTypeId = 5, ControlTypeName = "Multi-line Text Box" });
            tempControlTypesList.Add(new CategoryAttributesControlTypeViewModel { ControlTypeId = 6, ControlTypeName = "Date picker" });

            return tempControlTypesList;
        }

        /// <summary>
        /// Internal Method for Populating Search By List
        /// </summary>
        /// <returns></returns>
        protected internal List<CategoryAttributesSearchByViewModel> PopulateSearchByes()
        {
            var tempSearchByList = new List<CategoryAttributesSearchByViewModel>();

            tempSearchByList.Add(new CategoryAttributesSearchByViewModel { SearchById = 1, SearhcByName= "Multiple Choice" });
            tempSearchByList.Add(new CategoryAttributesSearchByViewModel { SearchById = 2, SearhcByName = "Range" });
            

            return tempSearchByList;
        }

    }


    /// <summary>
    /// Category Attribute Control Type Model View
    /// </summary>
    public class CategoryAttributesControlTypeViewModel
    {
        /// <summary>
        /// Default Values for Category Attribute Control Type View Model
        /// </summary>
        public CategoryAttributesControlTypeViewModel()
        {
            ControlTypeId = 0;
            ControlTypeName = string.Empty;
        }

        /// <summary>
        /// Control Type Id
        /// </summary>
        public int ControlTypeId { get; set; }

        /// <summary>
        /// Control Type Name
        /// </summary>
        public string ControlTypeName { get; set; }

    }

    /// <summary>
    /// Category Attributes Search By View Model
    /// </summary>
    public class CategoryAttributesSearchByViewModel
    {
        /// <summary>
        /// Search By Id
        /// </summary>
        public int SearchById { get; set; }
        /// <summary>
        /// Search By Name
        /// </summary>
        public string SearhcByName { get; set; }
    }
}
