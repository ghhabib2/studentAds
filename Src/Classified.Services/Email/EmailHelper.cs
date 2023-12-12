using System;
using System.Configuration;
using System.Net.Mail;

namespace Classified.Services.Email
{
    public class EmailHelper
    {
        public static bool SendEmail(string fromEmail, string toEmail, string subject, string body, string from)
        {
            var message = new MailMessage();

            var objSmtpDetails = GetSmtpDetails();
            try
            {

                var fromAddress = new MailAddress(fromEmail, from);
                message.From = fromAddress;
                message.To.Add(toEmail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                // Send SMTP mail
                var smtp = new SmtpClient(objSmtpDetails.SmtpServer)
                {
                    Credentials = new System.Net.NetworkCredential(objSmtpDetails.UserName, objSmtpDetails.Password),
                    Port = objSmtpDetails.PortNo,
                    EnableSsl = objSmtpDetails.EnableSsl
                };

                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                //log error
                return false;
            }
        }

        /// <summary>
        /// Class that define the SMTP Detail Model
        /// </summary>
        public class SmtpDetails
        {
            /// <summary>
            /// Enable SSL Property for SMTP SERVER
            /// </summary>
            public bool EnableSsl { get; set; }
            /// <summary>
            /// Port Number of SMTP SERVER
            /// </summary>
            public int PortNo { get; set; }
            /// <summary>
            /// SMTP Server Address
            /// </summary>
            public string SmtpServer { get; set; }
            /// <summary>
            /// SMTP Server User Name
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// SMTP Server Password
            /// </summary>
            public string Password { get; set; }
        }

        private static SmtpDetails GetSmtpDetails()
        {
            AppSettingsReader objAppSettingsReader = new AppSettingsReader();
            SmtpDetails objSmtpDetails = new SmtpDetails();
            objSmtpDetails.EnableSsl = Convert.ToBoolean(objAppSettingsReader.GetValue("smtpEnableSsl", typeof(string)));
            objSmtpDetails.UserName = objAppSettingsReader.GetValue("smtpUsername", typeof(string)).ToString();
            objSmtpDetails.Password = objAppSettingsReader.GetValue("smtpPassword", typeof(string)).ToString();
            objSmtpDetails.PortNo = Convert.ToInt32(objAppSettingsReader.GetValue("smtpPort", typeof(string)));
            objSmtpDetails.SmtpServer = objAppSettingsReader.GetValue("smtpServer", typeof(string)).ToString();
            return objSmtpDetails;
        }

        /// <summary>
        /// Classified Web site SMTP Client
        /// </summary>
        public static SmtpClient ClassifiedSmtpClient
        {
            get
            {
                var objSmtpDetails = GetSmtpDetails();
                return new SmtpClient(objSmtpDetails.SmtpServer)
                {
                    Credentials = new System.Net.NetworkCredential(objSmtpDetails.UserName, objSmtpDetails.Password),
                    Port = objSmtpDetails.PortNo,
                    EnableSsl = objSmtpDetails.EnableSsl,
                    Host = objSmtpDetails.SmtpServer
                };
            }
        }
    }
}