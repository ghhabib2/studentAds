using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classified.Component.Html;

namespace Classified.Domain.ViewModels.Base
{
    /// <summary>
    /// Basic View Model which has all the related elements that should be included in all models
    /// </summary>
    public class ClientViewModel
    {
        public ClientViewModel()
        {
            BreadcrumbList=new List<BreadcrumbViewModel>();
        }

        /// <summary>
        /// BreadCrubmb list for the pages
        /// </summary>
        public IEnumerable<BreadcrumbViewModel> BreadcrumbList { get; set; }
    }
    
}
