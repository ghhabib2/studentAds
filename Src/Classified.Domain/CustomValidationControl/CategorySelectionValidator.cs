using System.ComponentModel.DataAnnotations;
using Classified.Domain.ViewModels.Advertisment;

namespace Classified.Domain.CustomValidationControl
{
    /// <summary>
    /// Custom validation for 
    /// </summary>
    public class CategorySelectionValidationSubmitByEmailAddress: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            var advertisement =(AdvertisementEmailBasePrimaryRegisterationViewModel) validationContext.ObjectInstance ;

            if (advertisement.ClassifiedCategoryId != null)
            {
                // Check if the category Id is -1
                if (advertisement.ClassifiedCategoryId != -1)
                {//Return success message
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("You cannot select a parent category as a target category for your Advertisement. Your category should be selected from those categories that are not bold in the list.");
                }
            }
            else
            {
                return new ValidationResult("Selecting advertisement category is essential for this operation.");
            }
            
            
        }
    }

    /// <summary>
    /// Custom validation for 
    /// </summary>
    public class CategorySelectionValidationAdvertisementModification : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var advertisement = (ClassifiedAdvertisementModifyViewModel)validationContext.ObjectInstance;

            if (advertisement.ClassifiedCategoryId != null)
            {
                // Check if the category Id is -1
                if (advertisement.ClassifiedCategoryId != -1)
                {//Return success message
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("You cannot select a parent category as a target category for your Advertisement. Your category should be selected from those categories that are not bold in the list.");
                }
            }
            else
            {
                return new ValidationResult("Selecting advertisement category is essential for this operation.");
            }


        }
    }
}
