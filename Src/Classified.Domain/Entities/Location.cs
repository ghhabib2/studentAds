using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Classified.Domain.Entities
{
    public class Location
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Location Name")]
        [Required]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        public string Name { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Default Location")]
        public bool IsDefault { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string UrlName { get; set; }

        public int ParentId { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        //public virtual ICollection<ClassifiedAd> ClassifiedAds { get; set; }

        
    }
}