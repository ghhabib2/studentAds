using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Classified.Web.UserServices
{
    /// <summary>
    /// Application Settings of the Web-Site
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Web Site Name
        /// </summary>
        public static string SiteName => ConfigurationManager.AppSettings["SiteName"];

        /// <summary>
        /// Web Site Address
        /// </summary>
        public static string SiteAddress => ConfigurationManager.AppSettings["SiteAddress"];
    }
}