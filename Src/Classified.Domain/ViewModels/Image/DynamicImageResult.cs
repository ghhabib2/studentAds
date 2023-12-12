using System.Web.Mvc;
using System.Web;


namespace Classified.Domain.ViewModels.Image
{
    public class DynamicImageResult : FileContentResult
    {
        public DynamicImageResult(string fileName, byte[] fileData) : 
            base(fileData, string.Format("image/{0}", fileName.FileExtensionForContentType()))
        {
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.SetDefaultImageHeaders();
            base.WriteFile(response);
        }
    }
}