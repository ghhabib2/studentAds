using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Classified.Domain.ViewModels.Advertisment;

namespace Classified.Domain.ViewModels.Image
{
    /// <summary>
    /// Image View Model
    /// </summary>
    public class ImageViewModel
    {
        /// <summary>
        /// Id of the Image in Database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Image Name
        /// </summary>
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }

        /// <summary>
        /// Image Guide
        /// </summary>
        [Display(Name = "Image Guide/Alternative Text")]
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
        public ClassifiedAdvertisementViewModel ClassifiedAdvertisement { get; set; }

        /// <summary>
        /// Input Image File
        /// </summary>
        public HttpPostedFileBase inputImage { get; set; }
    }
}
