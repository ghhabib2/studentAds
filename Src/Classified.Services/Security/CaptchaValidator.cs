using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Classified.Services.Security
{
    public class CaptchaValidator
    {
        public static bool isValidated
        {
            get
            {
                AppSettingsReader objAppSettingsReader = new AppSettingsReader();
                var secret = objAppSettingsReader.GetValue("recaptchaPrivatekey", typeof(string)).ToString();

                var response = HttpContext.Current.Request.Form["g-Recaptcha-Response"];
                var client = new WebClient();
                var reply =
                    client.DownloadString(
                        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                            secret, response));

                CaptchaResponse captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                //when response is false check for the error message
                if (!captchaResponse.Success)
                {
                    // this means failed
                    return false;
                }
                else
                {
                    // this means they passed, let them continue
                    return true;
                }
            }
        }

        public class CaptchaResponse
        {

            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }

        }
    }
}
