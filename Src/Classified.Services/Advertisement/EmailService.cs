using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Classified.Services.Email;
using Quartz;

namespace Classified.Services.Advertisement
{
    /// <summary>
    /// Email services related to Advertisements
    /// </summary>
    public class EmailService:IEmailService
    {
        public bool EmailSubmitConfirmation(string emailAddress, string token)
        {
            //Read the related embedded resource as file stream
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Classified.Services.emailTemplates.AddEmailPrimarySubmit.html";

            string emailTemplate ;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    emailTemplate = reader.ReadToEnd();
                }

            }

            //Add the email address to the template.
            emailTemplate=emailTemplate.Replace("[emailAddress]", emailAddress);
            //generateConfirmation Link
            AppSettingsReader objAppSettingsReader = new AppSettingsReader();
            var hostAddress = objAppSettingsReader.GetValue("websiteAddress", typeof(string)).ToString();
            var address = $"{hostAddress}Advertisements/EmailBaseAdsConfirmation?email={emailAddress}&token={token}";
            //Replace generated Address in Template
            emailTemplate = emailTemplate.Replace("[confirmationLink]", address);

            var serviceEMailaddress = objAppSettingsReader.GetValue("emailServicesEmailAddress", typeof(string)).ToString();
            var emailServicesEmailAddressFrom =objAppSettingsReader.GetValue("emailServicesEmailAddressFrom", typeof(string)).ToString();

            if (EmailHelper.SendEmail(serviceEMailaddress,emailAddress,"Confirm your Advertisement",emailTemplate, emailServicesEmailAddressFrom))
            {
                return true;
            }

            return false;
        }

        public bool AdvertisementConfirmationEmail(string emailAddress, long id)
        {
            //Read the related embedded resource as file stream
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Classified.Services.emailTemplates.AdvertsementSubmittedByEmailModification.html";

            string emailTemplate;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    emailTemplate = reader.ReadToEnd();
                }

            }

            //Add the email address to the template.
            emailTemplate = emailTemplate.Replace("[emailAddress]", emailAddress);
            //generateConfirmation Link
            AppSettingsReader objAppSettingsReader = new AppSettingsReader();
            var hostAddress = objAppSettingsReader.GetValue("websiteAddress", typeof(string)).ToString();
            var address = $"{hostAddress}Advertisements/AdsEmailModification/{emailAddress}/{id}/1";
            //Replace generated Address in Template
            emailTemplate = emailTemplate.Replace("[confirmationLink]", address);

            var serviceEMailaddress = objAppSettingsReader.GetValue("emailServicesEmailAddress", typeof(string)).ToString();
            var emailServicesEmailAddressFrom = objAppSettingsReader.GetValue("emailServicesEmailAddressFrom", typeof(string)).ToString();

            if (EmailHelper.SendEmail(serviceEMailaddress, emailAddress, "Confirm your Advertisement", emailTemplate, emailServicesEmailAddressFrom))
            {
                return true;
            }

            return false;
        }

        public string ModificationLinkGenerator(string email,long id)
        {
            AppSettingsReader objAppSettingsReader = new AppSettingsReader();
            var hostAddress = objAppSettingsReader.GetValue("websiteAddress", typeof(string)).ToString();
            return $"{hostAddress}Advertisements/AdsEmailModification/{email}/{id}/1";
        }

        public bool AdvertisementFinalSubmission(long adsId, string emailAddress, string siteName)
        {
            //Read the related embedded resource as file stream
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Classified.Services.emailTemplates.AdvertisementEmailBaseFinalSubmission.html";

            string emailTemplate;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    emailTemplate = reader.ReadToEnd();
                }

            }

            //Add the email address to the template.
            emailTemplate = emailTemplate.Replace("[emailAddress]", emailAddress);
            //Add the Advertisement Id
            emailTemplate = emailTemplate.Replace("[AdvertisementId]", adsId.ToString());
            //Add the Advertisement SiteName
            emailTemplate = emailTemplate.Replace("[SiteName]", siteName);
            
            //generateConfirmation Link
            AppSettingsReader objAppSettingsReader = new AppSettingsReader();
            
            var serviceEMailaddress = objAppSettingsReader.GetValue("emailServicesEmailAddress", typeof(string)).ToString();
            var emailServicesEmailAddressFrom = objAppSettingsReader.GetValue("emailServicesEmailAddressFrom", typeof(string)).ToString();

            if (EmailHelper.SendEmail(serviceEMailaddress, emailAddress, $"Advertisement with Id:{adsId} has been submitted for review.", emailTemplate, emailServicesEmailAddressFrom))
            {
                return true;
            }

            return false;
        }

        public bool EmailBasedAdvertisementApprovement(long adsId, string emailAddress)
        {
            //Read the related embedded resource as file stream
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Classified.Services.emailTemplates.AddEmailBaseConfirm.html";

            string emailTemplate;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    emailTemplate = reader.ReadToEnd();
                }

            }

            //Add the email address to the template.
            emailTemplate = emailTemplate.Replace("[emailAddress]", emailAddress);
            //Add the Advertisement Id
            emailTemplate = emailTemplate.Replace("[AdvertisementId]", adsId.ToString());
            //Add the Advertisement SiteName
            emailTemplate = emailTemplate.Replace("[adsLink]", ModificationLinkGenerator(emailAddress,adsId));

            //generateConfirmation Link
            AppSettingsReader objAppSettingsReader = new AppSettingsReader();

            var serviceEMailaddress = objAppSettingsReader.GetValue("emailServicesEmailAddress", typeof(string)).ToString();
            var emailServicesEmailAddressFrom = objAppSettingsReader.GetValue("emailServicesEmailAddressFrom", typeof(string)).ToString();

            if (EmailHelper.SendEmail(serviceEMailaddress, emailAddress, $"Advertisement with Id:{adsId} has been approved.", emailTemplate, emailServicesEmailAddressFrom))
            {
                return true;
            }

            return false;
        }

        public bool EmailBasedAdvertisementRejection(long adsId, string emailAddress)
        {
            //Read the related embedded resource as file stream
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Classified.Services.emailTemplates.AddEmailBaseConfirm.html";

            string emailTemplate;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    emailTemplate = reader.ReadToEnd();
                }

            }

            //Add the email address to the template.
            emailTemplate = emailTemplate.Replace("[emailAddress]", emailAddress);
            //Add the Advertisement Id
            emailTemplate = emailTemplate.Replace("[AdvertisementId]", adsId.ToString());
            //Add the Advertisement SiteName
            emailTemplate = emailTemplate.Replace("[adsLink]", ModificationLinkGenerator(emailAddress, adsId));

            //generateConfirmation Link
            AppSettingsReader objAppSettingsReader = new AppSettingsReader();

            var serviceEMailaddress = objAppSettingsReader.GetValue("emailServicesEmailAddress", typeof(string)).ToString();
            var emailServicesEmailAddressFrom = objAppSettingsReader.GetValue("emailServicesEmailAddressFrom", typeof(string)).ToString();

            if (EmailHelper.SendEmail(serviceEMailaddress, emailAddress, $"Advertisement with Id:{adsId} has been rejected.", emailTemplate, emailServicesEmailAddressFrom))
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Interface for Advertisement email Service
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send Confirmation Email to customer about submitted advertisement
        /// </summary>
        /// <param name="emailAddress">Customer Email Address</param>
        /// <param name="token">Automatic generated token</param>
        /// <param name="adsId">Advertisement Id</param>
        /// <returns></returns>
         bool EmailSubmitConfirmation(string emailAddress, string token);

        /// <summary>
        /// Send an email to the user to inform him/her that the advertisement confirmed with a link to access the advertisement.
        /// </summary>
        /// <param name="emailAddress">Customer email address</param>
        /// <param name="id">Advertisement Id</param>
        /// <returns></returns>
        bool AdvertisementConfirmationEmail(string emailAddress, long id);

        /// <summary>
        /// Generate a Modification Link for the user to have access to his/her advertisement
        /// </summary>
        /// <param name="id">Advertisement Id</param>
        /// <returns></returns>
        string ModificationLinkGenerator(string email,long id);

        /// <summary>
        /// Send an email to user to inform him/her about the submission of his/her ads for final review
        /// </summary>
        /// <param name="adsId">Advertisement Id</param>
        /// <param name="emailAddress">Email Address</param>
        /// <param name="siteName">Web SiteName</param>
        /// <returns></returns>
        bool AdvertisementFinalSubmission(long adsId, string emailAddress, string siteName);

        /// <summary>
        /// Send the Approvement email address to the user
        /// </summary>
        /// <param name="adsId">Advertisement Id</param>
        /// <param name="emailAddress">Advertisement Provider Email Address</param>
        /// <returns>True if the email send operation is successful</returns>
        bool EmailBasedAdvertisementApprovement(long adsId, string emailAddress);

        /// <summary>
        /// Send the Approvement email address to the user
        /// </summary>
        /// <param name="adsId">Advertisement Id</param>
        /// <param name="emailAddress">Advertisement Provider Email Address</param>
        /// <returns>True if the email send operation is successful</returns>
        bool EmailBasedAdvertisementRejection(long adsId, string emailAddress);
    }
}
