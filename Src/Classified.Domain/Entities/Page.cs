using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Classified.Domain.Entities
{
    public class Page
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        public string Name { get; set; }

        [Required]
        public string Title { get; set; }

       // [Required]
        [AllowHtml]
        [StringLength(10000000)]
        public string Body { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOnUtc { get; set; }

        [Display(Name = "Created On")]
        public DateTime? UpdatedOnUtc { get; set; }

        [Display(Name = "Meta Title")]
        //[Required]
        public string MetaTitle { get; set; }

        [Display(Name = "Meta Keywords")]
        //[Required]
        public string MetaKeywords { get; set; }

        [Display(Name = "Meta Desc")]
       // [Required]
        public string MetaDescription { get; set; }

        [Display(Name = "Page Desc")]
        //[Required]
        public string PageDesc { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Default ")]
        public bool IsDefault { get; set; }
    }
}