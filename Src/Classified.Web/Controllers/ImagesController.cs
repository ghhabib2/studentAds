using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Classified.Data.Advertisements.Advertisement;
using Classified.Data.Advertisements.Categories;
using Classified.Data.Advertisements.Images;
using Classified.Domain.ViewModels;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Domain.ViewModels.Image;
using Classified.Services;
// Image Resizer is the library we are not going to use anymore for Image manipulation
//using ImageResizer;
// Kaliko's Image Library is one of our alternatives for Image Resizer
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;


namespace Classified.Web.Controllers
{
    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImagesController : Controller
    {

        /// <summary>
        /// Delete an image based on the name of the image
        /// </summary>
        /// <param name="model">Image model View</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ImageViewModel model)
        {
            //Find the image in the database and delete its information
            if (new ImageCore().Delete(item => item.Id == model.Id))
            {
                //File information deleted from Database. Delete the file itself.
                System.IO.File.Delete(getFullFilePath(model.ImageName));

                //Write a Success Message for the user
                PopUpMessageGenerator.GenerateMessage("File Management System",
                    "Image Deleted successfully.",
                    MessageStatus.Successfull);
            }
            else
            {
                PopUpMessageGenerator.GenerateMessage("File Management System",
                    "There is a problem in deleting your image information. Please wait a few minutes and try again. Please contact our development team if you see this message again.",
                    MessageStatus.Failed);
            }

            return RedirectToRoute("AdsEmailModification", new { id = Request["AdsId"], email = Request["EmailAddress"], state = 4 });
        }

        /// <summary>
        /// Upload Image into Ads Database
        /// </summary>
        /// <param name="file">Image file the uploaded to the database</param>
        /// <param name="model">Image View Model</param>
        /// <returns>Return to the Form</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadAdsImageEmail(ImageViewModel model)
        {
            //Upload Image.

            //Check if the file type is an image
            var result = ImageState(model.inputImage);

            if (result != FileContentState.IsImage)
            {
                //The file is not an image. Return a related Error to the form
                switch (result)
                {
                    case FileContentState.NotValidImage:
                        {
                            PopUpMessageGenerator.GenerateMessage("File Management System", "The file is not a valid image.", MessageStatus.Warning);
                            break;
                        }
                    case FileContentState.ExceedMinimumSize:
                        {
                            PopUpMessageGenerator.GenerateMessage("File Management System", "The uploaded image size should be less than or equal to 352 kilobyte.", MessageStatus.Warning);
                            break;
                        }
                    case FileContentState.CanNotRead:
                        {
                            PopUpMessageGenerator.GenerateMessage("File Management System", "Cannot read the file of image.", MessageStatus.Warning);
                            break;
                        }
                    case FileContentState.IsNotImage:
                        {
                            PopUpMessageGenerator.GenerateMessage("File Management System", "The file type is not a known image file.", MessageStatus.Warning);
                            break;
                        }
                }

                //Return to the form again
                return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"], state = 4 });
            }

            //The file is an image, upload it into server
            var imageFileName = Guid.NewGuid() + Path.GetExtension(model.inputImage.FileName);

            if (model.inputImage.ContentLength > 0)
            {
                model.inputImage.SaveAs(Path.Combine(Server.MapPath("~/Content/Images"), imageFileName));
            }

            //Add its information into database
            var tempImageInfo = new ImageViewModel
            {
                ClassifiedAdvertisementId = Convert.ToInt64(Request["Id"]),
                Deleted = false,
                ImageGuid = Request["ImageGuid"],
                ImageName = imageFileName,
                UploadedOnUtc = DateTime.UtcNow,
            };

            //Add file information to database
            if (new ImageCore().Insert(tempImageInfo))
            {
                //Redirect to the View of Modification with State Modification in Images
                //Show Success message
                PopUpMessageGenerator.GenerateMessage("File Management System",
                    "File uploaded successfully.", MessageStatus.Successfull);
                //Redirect to the Route
                return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"], state = 4 });
            }
            else
            {
                //There is a problem so delete the file
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/Images"), imageFileName));

                //Show Failed Error Message
                PopUpMessageGenerator.GenerateMessage("File Management System",
                    "There is a problem on storing your file information in our database. Please try again a few minutes later. Please contact our development team if ypu see this message again",
                    MessageStatus.Failed);
                //Redirect to the Route
                return RedirectToRoute("AdsEmailModification", new { id = Request["Id"], email = Request["EmailAddress"], state = 4 });
            }

        }

        /// <summary>
        /// Returning Action Result in the form of File Results. It creates the full path of File in the server and return it in the from of FielResult
        /// </summary>
        /// <param name="file">Image file</param>
        /// <returns>File Result with it's content type if the file exist. Error 404 with a relative text if the file does not exist</returns>
        public ActionResult Render(string file)
        {
            var fullFilePath = this.getFullFilePath(file);
            if (this.imageFileNotAvailable(fullFilePath))
                return this.instantiate404ErrorResult(file);
            return new ImageFileResult(fullFilePath);
        }

        public ActionResult RenderImage(string id)
        {
            var fullFilePath = this.getFullFilePath(id);
            if (this.imageFileNotAvailable(fullFilePath))
                return this.instantiate404ErrorResult(id);

            // Get the watermark image's full path
            var watermarkFullFilePath = this.getFullFilePath("watermark.png");

            // Get the watermark image's full path
            //var watermarkFullFilePath = this.getFullFilePath("Watermark.png");

            // Watermark the target image
            //var watermarkedImage = this.addWatermark(image, watermarkFullFilePath);

            var finalImage = addWatermark(new KalikoImage(fullFilePath), watermarkFullFilePath);

            var ms = new MemoryStream();

            finalImage.SaveJpg(ms, 100);

            //var imageByteArray = System.IO.File.ReadAllBytes(fullFilePath);

            return new DynamicImageResult(id, ms.ToArray());
        }


        /// <summary>
        /// Resize a target image to desired width and height
        /// </summary>
        /// <param name="width">Width of the New Image</param>
        /// <param name="height">Height of the New Image</param>
        /// <param name="folder">Folder Name that contain the file of target image</param>
        /// <param name="file">File name of target image</param>
        /// <returns>Return the Resized Image int he form of FileContentResult</returns>
        public ActionResult RenderWithResize(int width, int height, string folder, string file)
        {
            // Generates the fiel full path based on the selected folder and file name
            var fullFilePath = this.getFullFilePathNew(file, folder);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);

            // Read the Image File
            var image = new KalikoImage(fullFilePath);

            // Line of code had been written in order to prepare the image for resize operation
            //var resizeSettings = this.instantiateResizeSettings(width, height);

            // Line of code had been written in order to build the resized image
            //var resizedImage = ImageBuilder.Current.Build(fullFilePath, resizeSettings);

            //New line of code we are using for resizing the image
            image.Resize(width, height);

            // Line of code had been written in order to return the byte array of the resized image
            //return new DynamicImageResult(file, resizedImage.ToByteArray());

            var ms = new MemoryStream();

            image.SaveJpg(ms, 100);

            //New line of code we are using for returning the resized image
            return new DynamicImageResult(file, ms.ToArray());
        }


        /// <summary>
        /// Resize the profile image to desired with and height
        /// </summary>
        /// <param name="width">Width of the New Image</param>
        /// <param name="height">Height of the New Image</param>
        /// <param name="file">File name of target image</param>
        /// <returns>Return the Resized Image int he form of FileContentResult</returns>
        public ActionResult RenderWithResizeProfile(int width, int height, string file)
        {
            // Generates the fiel full path based on the selected file name
            var fullFilePath = this.getFullFilePath(file);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);

            // Read the Image File
            var image = new KalikoImage(fullFilePath);


            // Line of code had been written in order to prepare the image for resize operation
            //var resizeSettings = this.instantiateResizeSettings(width, height);

            // Line of code had been written in order to build the resized image
            //var resizedImage = ImageBuilder.Current.Build(fullFilePath, resizeSettings);

            //New line of code we are using for resizing the image
            image.Resize(width, height);

            var ms = new MemoryStream();

            image.SaveJpg(ms, 100);

            // Line of code had been written in order to return the byte array of the resized image
            //return new DynamicImageResult(file, resizedImage.ToByteArray());

            //New line of code we are using for returning the resized image
            return new DynamicImageResult(file, ms.ToArray());
        }


        /// <summary>
        /// Change the Resolution of the target Image
        /// </summary>
        /// <param name="file">File Name</param>
        /// <param name="targetResolution">Target resolution</param>
        /// <returns>return Image that its resolution changed in the form of FileContentResult</returns>
        public ActionResult RenderWithResolutionChange(string file, float targetResolution)
        {
            // Generates the fiel full path based on the selected file name
            var fullFilePath = this.getFullFilePath(file);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);

            // Read the Image File
            var image = new KalikoImage(fullFilePath);


            //Change the resolution of the image
            image.HorizontalResolution = targetResolution;
            image.VerticalResolution = targetResolution;

            var ms = new MemoryStream();

            image.SaveJpg(ms, 100);

            //return the manipulated image
            return new DynamicImageResult(file, ms.ToArray());
        }


        /// <summary>
        /// Change the Resolution of the target Image with different parameters for Horizontal and Vertical values
        /// </summary>
        /// <param name="file">File Name</param>
        /// <param name="horizontalResolution">Target horizontal resolution</param>
        /// <param name="verticalResolution">Target vertical resolution</param>
        /// <returns>return Image that its resolution changed in the form of FileContentResult</returns>
        public ActionResult RenderWithResolutionChange(string file, float horizontalResolution, float verticalResolution)
        {
            // Generates the fiel full path based on the selected file name
            var fullFilePath = this.getFullFilePath(file);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);

            // Read the Image File
            var image = new KalikoImage(fullFilePath);


            //Change the resolution of the image
            image.HorizontalResolution = horizontalResolution;
            image.VerticalResolution = verticalResolution;

            var ms = new MemoryStream();

            image.SaveJpg(ms, 100);

            //return the manipulated image
            return new DynamicImageResult(file, ms.ToArray());
        }


        /// <summary>
        /// Change the Resolution of the target Image with different parameters for Horizontal and Vertical values and Resize the image in the same time
        /// </summary>
        /// <param name="file">File Name</param>
        /// <param name="horizontalResolution">Target horizontal resolution</param>
        /// <param name="verticalResolution">Target vertical resolution</param>
        /// <param name="height">The disered height for the image</param>
        /// <param name="width">The disered width for the image</param>
        /// <returns>return Image that its resolution and size changed in the form of FileContentResult</returns>
        public ActionResult RenderWithResolutionChange(string file, float horizontalResolution, float verticalResolution, int width, int height)
        {
            // Generates the fiel full path based on the selected file name
            var fullFilePath = this.getFullFilePath(file);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);

            // Read the Image File
            var image = new KalikoImage(fullFilePath);

            //Set the desired size for the image
            image.Resize(width, height);

            //Change the resolution of the image
            image.HorizontalResolution = horizontalResolution;
            image.VerticalResolution = verticalResolution;

            var ms = new MemoryStream();

            image.SaveJpg(ms, 100);

            //return the manipulated image
            return new DynamicImageResult(file, ms.ToArray());
        }

        /// <summary>
        /// Watermark a target image based on the desired width and height in the form of Tile water mark
        /// </summary>
        /// <param name="id">File path of the target image</param>
        /// <returns></returns>
        public ActionResult Watermarked(string id)
        {
            // Generates the file full path based on the selected file name
            var fullFilePath = this.getFullFilePath(id);

            // Check if the file available in the target folder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(id);

            // Previous implementation for image resizing
            /*
            var resizeSettings = this.instantiateResizeSettings(width, height);
            var resizedImage = ImageBuilder.Current.Build(fullFilePath, resizeSettings);
            */

            //Load target image
            var image = new KalikoImage(fullFilePath);

            // Get the watermark image's full path
            var watermarkFullFilePath = this.getFullFilePath("Watermark.png");

            // Watermark the target image
            var watermarkedImage = this.addWatermark(image, watermarkFullFilePath);

            var ms = new MemoryStream();

            watermarkedImage.SaveJpg(ms, 100);

            // Return the final result as ByteArray
            return new DynamicImageResult(id, ms.ToArray());
        }


        /// <summary>
        /// Watermark a target image based on the desired width and height in the form of Tile water mark
        /// </summary>
        /// <param name="width">width of the target image</param>
        /// <param name="height">height of the target image</param>
        /// <param name="file">File path of the target image</param>
        /// <returns></returns>
        public ActionResult RenderWithResizeAndWatermark(string Id, int width, int height)
        {
            // Generates the file full path based on the selected file name
            var fullFilePath = this.getFullFilePath(Id);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(Id);

            // Previous implementation for image resizeing
            /*
            var resizeSettings = this.instantiateResizeSettings(width, height);
            var resizedImage = ImageBuilder.Current.Build(fullFilePath, resizeSettings);
            */

            //Load target image
            var image = new KalikoImage(fullFilePath);

            //New implementation for resizing an image
            image.Resize(width, height);

            // Get the watermark image's full path
            var watermarkFullFilePath = this.getFullFilePath("Watermark.png");

            // Watermark the target image
            var watermarkedImage = this.addWatermark(image, watermarkFullFilePath);

            var ms = new MemoryStream();

            watermarkedImage.SaveJpg(ms, 100);

            // Return the final result as ByteArray
            return new DynamicImageResult(Id, ms.ToArray());
        }


        /// <summary>
        /// Watermark a target image based on the desired width and height on an specific point of the image
        /// </summary>
        /// <param name="width">Width of the target image</param>
        /// <param name="height">Height of the target image</param>
        /// <param name="file">File path of source image</param>
        /// <param name="watermarkLocation">WatermarkLocation</param>
        /// <returns></returns>
        public ActionResult RenderWithResizeAndWatermark(string Id, int width, int height, Point watermarkLocation)
        {
            // Generates the fiel full path based on the selected file name
            var fullFilePath = this.getFullFilePath(Id);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(Id);

            // Previous implementation for image resizeing
            /*
            var resizeSettings = this.instantiateResizeSettings(width, height);
            var resizedImage = ImageBuilder.Current.Build(fullFilePath, resizeSettings);
            */

            //Load target image
            var image = new KalikoImage(fullFilePath);

            //New implenetation for resizing an image
            image.Resize(width, height);

            // Get the watermark image's full path
            var watermarkFullFilePath = this.getFullFilePath("watermark.png");

            // Watermark the target image
            var watermarkedImage = this.addWatermark(image, watermarkFullFilePath, watermarkLocation);

            var ms = new MemoryStream();

            watermarkedImage.SaveJpg(ms, 100);

            // Return the final result as ByteArray
            return new DynamicImageResult(Id, ms.ToArray());
        }

        /// <summary>
        /// Return an Thumbnail of the image with new size by cropping the source image
        /// Attention : this function may not make any changes on scales of theimage if the desired size is smaller than or equal to the source image original size
        /// </summary>
        /// <param name="file">Image's file name</param>
        /// <param name="width">Target image's width</param>
        /// <param name="height">Target image's height</param>
        /// <returns>Return the cropped image based on its new size</returns>
        public ActionResult RenderWithThumbnailByCropping(string file, int width, int height)
        {
            // Generates the fiel full path based on the selected file name
            var fullFilePath = this.getFullFilePath(file);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);

            //Load the image
            var image = new KalikoImage(fullFilePath);

            //Check the size of the image
            width = (image.Width <= width ? image.Width : width);
            height = (image.Height <= height ? image.Height : height);

            var thumb = image.Scale(new CropScaling(width, height));

            var ms = new MemoryStream();

            thumb.SaveJpg(ms, 100);

            // Return the final result as ByteArray
            return new DynamicImageResult(file, ms.ToArray());
        }


        /// <summary>
        /// Return an Thumbnail of the image with new size by Padding the source image
        /// Attention : this function may not make any changes on scales of theimage if the desired size is smaller than or equal to the source image original size
        /// </summary>
        /// <param name="file">Image's file name</param>
        /// <param name="width">Target image's width</param>
        /// <param name="height">Target image's height</param>
        /// <param name="backgroundColor">Background Color for the empty part of image</param>
        /// <returns>Return the cropped image based on its new size</returns>
        public ActionResult RenderWithThumbnailByPadding(string file, int width, int height, System.Drawing.Color backgroundColor)
        {
            // Generates the fiel full path based on the selected file name
            var fullFilePath = this.getFullFilePath(file);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);



            //Load the image
            var image = new KalikoImage(fullFilePath);

            //Check the size of the image
            width = (image.Width <= width ? image.Width : width);
            height = (image.Height <= height ? image.Height : height);

            var thumb = image.Scale(new PadScaling(width, height, backgroundColor));

            var ms = new MemoryStream();

            thumb.SaveJpg(ms, 100);

            // Return the final result as ByteArray
            return new DynamicImageResult(file, ms.ToArray());
        }




        /// <summary>
        /// Return an Thumbnail of the image with new size by fitting the source image
        /// Attention : this function may not make any changes on scales of theimage if the desired size is smaller than or equal to the source image original size. 
        /// This may result in a ratio that is different from the dimensions defined in the function call.
        /// </summary>
        /// <param name="file">Image's file name</param>
        /// <param name="width">Target image's width</param>
        /// <param name="height">Target image's height</param>
        /// <returns>Return the cropped image based on its new size</returns>
        public ActionResult RenderWithThumbnailByFitting(string file, int width, int height)
        {
            // Generates the file full path based on the selected file name
            var fullFilePath = this.getFullFilePath(file);

            // Check if the file available in the target forlder
            if (this.imageFileNotAvailable(fullFilePath))
                // Return the 404 Error if file does not exist
                return this.instantiate404ErrorResult(file);

            //Load the image
            var image = new KalikoImage(fullFilePath);

            //Check the size of the image
            width = (image.Width <= width ? image.Width : width);
            height = (image.Height <= height ? image.Height : height);

            var thumb = image.Scale(new FitScaling(width, height));

            var ms = new MemoryStream();

            thumb.SaveJpg(ms, 100);

            // Return the final result as ByteArray
            return new DynamicImageResult(file, ms.ToArray());
        }



        /// <summary>
        /// Generates the file full path
        /// </summary>
        /// <param name="file">File Name</param>
        /// <returns>Return the file full path in the server</returns>
        private string getFullFilePath(string file)
        {
            return string.Format("{0}/{1}", Server.MapPath("~/Content/Images"), file);
        }

        /// <summary>
        /// Generates file full path based on the target folder and file name
        /// </summary>
        /// <param name="file">File name</param>
        /// <param name="folder">Folder name</param>
        /// <returns>Return the file path in the server</returns>
        private string getFullFilePathNew(string file, string folder)
        {
            return string.Format("{0}/{1}", Server.MapPath("~/Content/Images" + folder), file);
        }

        /// <summary>
        /// Check the availability of file in the server based on the target address
        /// </summary>
        /// <param name="fullFilePath">File full path</param>
        /// <returns>True if the file exist and False if there is no such a file in the desired address</returns>
        private bool imageFileNotAvailable(string fullFilePath)
        {
            return !System.IO.File.Exists(fullFilePath);
        }

        /// <summary>
        /// Return the 404 Error as Action Result when ther is no file in selected address
        /// </summary>
        /// <param name="file">File's full path</param>
        /// <returns>Return the HttpNotFoundResult as ActionResult if the function invoked</returns>
        private HttpNotFoundResult instantiate404ErrorResult(string file)
        {
            return new HttpNotFoundResult(string.Format("The file {0} does not exist.", file));
        }

        /// <summary>
        /// Function implemented for watermarking a target image
        /// </summary>
        /// <param name="image">Target Image</param>
        /// <param name="watermarkFullFilePath">Watermark file full path</param>
        /// <returns></returns>
        private KalikoImage addWatermark(KalikoImage image, string watermarkFullFilePath)
        {
            //New watermarking implementation
            using (var watermark = new KalikoImage(watermarkFullFilePath))
            {
                var watermarkToUse = watermark;
                // Check the size of watermark image in order to make sure that the size of it is not bigger than the size of target image
                if (watermark.Width > image.Width || watermark.Height > image.Height)
                {
                    // Resize the watermark image if the size is bigger than the size of target iamge
                    watermarkToUse.Resize(image.Width, image.Height);
                }

                //Add Watermark to the image
                image.BlitFill(watermarkToUse);

            }

            // Return the watermarked image
            return image;

        }


        // <summary>
        // Function implemented for watermarking a target image on an specific location of the source image
        // </summary>
        // <param name = "image" > Target Image</param>
        // <param name = "watermarkFullFilePath" > Watermark file full path</param>
        // <param name = "watermarkLocation" > The address for saving watermarked file</param>
        // <returns></returns>
        private KalikoImage addWatermark(KalikoImage image, string watermarkFullFilePath, Point watermarkLocation)
        {

            //New watermarking implementation
            using (var watermark = new KalikoImage(watermarkFullFilePath))
            {
                var watermarkToUse = watermark;
                // Check the size of watermark image in order to make sure that the size of it is not bigger than the size of target image
                if (watermark.Width > image.Width || watermark.Height > image.Height)
                {
                    // Resize the watermark image if the size is bigger than the size of target iamge
                    watermarkToUse.Resize(image.Width, image.Height);
                }

                //Add Watermark to the image
                image.BlitImage(watermarkToUse, watermarkLocation.X, watermarkLocation.Y);

            }

            // Return the watermarked image
            return image;
        }

        protected enum FileContentState
        {
            IsImage = 1,
            IsNotImage = 2,
            CanNotRead = 3,
            ExceedMinimumSize = 4,
            NotValidImage = 5
        }

        /// <summary>
        /// Constant showing the minimum size of image in byte
        /// </summary>
        protected const int ImageMinimumBytes = 352000;

        ///Check if the uploaded file is an image
        protected FileContentState ImageState(HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "inputFile" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return FileContentState.IsNotImage;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return FileContentState.IsNotImage;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return FileContentState.CanNotRead;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.ContentLength > ImageMinimumBytes)
                {
                    return FileContentState.ExceedMinimumSize;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.InputStream.Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return FileContentState.NotValidImage;
                }
            }
            catch (Exception)
            {
                return FileContentState.NotValidImage;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return FileContentState.NotValidImage;
            }
            finally
            {
                postedFile.InputStream.Position = 0;
            }

            //The file is an image
            return FileContentState.IsImage;
        }

        //  ========================================================================================================================
        //  ====                                                                                                                ====
        //  ====    Functions that we are not using anymore due to the removal of ImageResizer image manipulation library       ====
        //  ====                                                                                                                ====
        //  ========================================================================================================================

        ///// <summary>
        /////  Class for getting the resize setting based on image resizer implementation. We are not using this function anymore
        ///// </summary>
        ///// <param name="width"></param>
        ///// <param name="height"></param>
        ///// <returns></returns>
        //private ResizeSettings instantiateResizeSettings(int width, int height)
        //{
        //    var queryString = string.Format("maxwidth={0}&maxheight={1}&quality=90", width, height);
        //    return new ResizeSettings(queryString);
        //}

        /// <summary>
        /// Function implemented for watermarking a target image
        /// </summary>
        /// <param name="image">Targer Image</param>
        /// <param name="watermarkFullFilePath">Watermark file full path</param>
        /// <param name="watermarkLocation">The address for saving watermarked file</param>
        /// <returns></returns>
        //private Bitmap addWatermark(KalikoImage image, string watermarkFullFilePath, Point watermarkLocation)
        //{
        //Previous Implementation of watermarking 
        //using (var watermark = Image.FromFile(watermarkFullFilePath))
        //{
        //    var watermarkToUse = watermark;
        //    if (watermark.Width > image.Width || watermark.Height > image.Height)
        //    {
        //        var resizeSettings = this.instantiateResizeSettings(image.Width, image.Height);
        //        watermarkToUse = ImageBuilder.Current.Build(watermarkFullFilePath, resizeSettings);
        //    }
        //    using (var graphics = Graphics.FromImage(image))
        //    {
        //        graphics.DrawImage(watermarkToUse, watermarkLocation);
        //    }
        //}
        //return image;
        //}
    }
}