using System;
using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
    public class ClassifiedImages
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Image Name
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Image Guide
        /// </summary>
        public string ImageGuid { get; set; }

        /// <summary>
        /// Flag for checking if the Image is Deleted
        /// </summary>
        public bool Deleted { get; set; }
       
        /// <summary>
        /// Upload Date
        /// </summary>
        public DateTime UploadedOnUtc { get; set; }
      
        /// <summary>
        /// Parent Advertisement Id
        /// </summary>
        public long ClassifiedAdvertisementId { get; set; }

        /// <summary>
        /// Parent Advertisement Object
        /// </summary>
        public ClassifiedAdvertisement ClassifiedAdvertisement { get; set; }

    }
}
