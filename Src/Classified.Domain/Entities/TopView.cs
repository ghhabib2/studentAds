using System;
using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
    public class TopView
    {
        [Key]
        public int Id { get; set; }

        public bool IsFeaturePost { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ClassifiedAdId { get; set; }

        public virtual ClassifiedAdvertisement ClassifiedAd { get; set; }

    }
}
