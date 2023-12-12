using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
   public class ClassifiedCategoryAttribute
    {

       [Key]
       public int Id { get; set; }

       [Required]
       public string AttributeLabel { get; set; }

       [Required]
       [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
       public string AttributeName { get; set; }

       [Display(Name = "Search By")]
       public int AttributeSearchBy { get; set; }

       [Display(Name = "Control Type")]
       public int AttributeControlTypeId { get; set; }

       public int ClassifiedCategoryId { get; set; }

       public ClassifiedCategory ClassifiedCategory { get; set; }

       //public virtual List<AttributeValue> AttributeValues { get; set; }

      
      
    }

    /// <summary>
    /// Class that represent the Attribute
    /// </summary>
    public class ClassifiedCategoryAttributeValue
    {
        /// <summary>
        /// Id of the Attribute's Value
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Attribute's Value
        /// </summary>
        [Required(ErrorMessage = "Entering value of attribute is necessary for this operation!!")]
        public string AttributeValue { get; set; }

        /// <summary>
        /// Attribute's value parent category id
        /// </summary>
        public int ClassifiedCategoryAttributeId { get; set; }

        /// <summary>
        /// Attribute's value parent category object
        /// </summary>
        public ClassifiedCategoryAttribute ClassifiedCategoryAttribute { get; set; }
        
    }

    public class FunctionClassifiedAdvertisementAttribute
    {
        /// <summary>
        /// Id of the Attribute
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Label of the Attribute
        /// </summary>
        public string AttributeLabel { get; set; }
        /// <summary>
        /// Value of the Attribute
        /// </summary>
        public string AttributeValue { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ClassifiedAdvertisementAttribute
    {
        /// <summary>
        /// Id and the primary Key of the Classified Ads Attributes
        /// </summary>
        [Key]
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
        /// Parent Classified Category Attribute Object
        /// </summary>
        public ClassifiedCategoryAttribute ClassifiedCategoryAttribute { get; set; }

        /// <summary>
        /// Parent Classified Advertisement Id
        /// </summary>
        public long ClassifiedAdvertisementId { get; set; }

        /// <summary>
        /// Parent Classified Advertisement Object
        /// </summary>
        public ClassifiedAdvertisement ClassifiedAdvertisement { get; set; }

    }
}
