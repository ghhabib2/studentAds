using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classified.Domain.Entities
{
    public class UserClassifiedMapping
    {
        [Key]
        public long Id { get; set; }

        public long ClassifiedAdId { get; set; }
        public ClassifiedAdvertisement ClassifiedAd { get; set; }

        public  string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
