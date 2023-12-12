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
    /// Category Attribute Values View Model Class
    /// </summary>
    public class CategoryAttributeValuesViewModel
    {

        public CategoryAttributeValuesViewModel()
        {
            Id = -1;
            AttributeValue = string.Empty;
            ClassifiedCategoryAttributeId = null;
            ClassifiedCategoryAttribute = null;
        }

        /// <summary>
        /// Id of the Attribute's Value
        /// </summary>
        [Key]
        [HiddenInput]
        public int Id { get; set; }

        /// <summary>
        /// Attribute's Value
        /// </summary>
        [Required(ErrorMessage = "Entering value of attribute is necessary for this operation!!")]
        [Display(Name = "Attribute Value")]
        public string AttributeValue { get; set; }

        /// <summary>
        /// Attribute's value parent category id
        /// </summary>
        public int? ClassifiedCategoryAttributeId { get; set; }

        /// <summary>
        /// Attribute's value parent category object
        /// </summary>
        public CategoryAttributesViewModel ClassifiedCategoryAttribute { get; set; }
    }

    /// <summary>
    /// View Model that will be used for management of Category Attribute Values
    /// </summary>
    public class CategoryAttributeValuesManagementViewModelate
    {

        public CategoryAttributeValuesManagementViewModelate()
        {
            CategoryList=new List<CategoryItemViewMoel>();
            CategoryId = -1;
            AttributesList=new List<CategoryAttributesViewModel>();
            CategoryAttributeId = -1;
            AttributeValues=new List<CategoryAttributeValuesViewModel>();
            AttributeValue=new CategoryAttributeValuesViewModel();
            AttributeValueId = -1;

        }

        /// <summary>
        /// List the represent hierarchy list of categories for the user
        /// </summary>
        public IEnumerable<CategoryItemViewMoel> CategoryList { get; set; }

        /// <summary>
        /// Target Category Id
        /// </summary>
        [Display(Name = "Target Category")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Target Attributes List
        /// </summary>
        public IEnumerable<CategoryAttributesViewModel> AttributesList { get; set; }

        /// <summary>
        /// Category Attribute Id that will be used in the View
        /// </summary>
        [Display(Name = "Target Category Attribute")]
        public int CategoryAttributeId { get; set; }

        /// <summary>
        /// List of Attribute values in the Form filtered based on Attribute Id
        /// </summary>
        public IEnumerable<CategoryAttributeValuesViewModel> AttributeValues { get; set; }

        /// <summary>
        /// Attribute Value that will be used in the form for inert and update operations
        /// </summary>
        public CategoryAttributeValuesViewModel AttributeValue { get; set; }

        /// <summary>
        /// Attribute Value Id that will be used for fetching the information of current Value.
        /// </summary>
        public int AttributeValueId { get; set; }

    }
}
