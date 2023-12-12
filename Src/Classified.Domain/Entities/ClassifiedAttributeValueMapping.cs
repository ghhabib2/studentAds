using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classified.Domain.Entities
{
    /// <summary>
    /// Removed in new Version
    /// </summary>
    public class ClassifiedAttributeValueMapping
    {
        [Key]
        public long Id { get; set; }

        public long ClassifiedAdId { get; set; }
        public ClassifiedAdvertisement ClassifiedAd { get; set; }

        public int AttributeValueId { get; set; }
        public ClassifiedCategoryAttributeValue AttributeValue { get; set; }
    }
}
