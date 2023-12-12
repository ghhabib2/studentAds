using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Classified.Component.Html
{

    /// <summary>
    /// View Model for Breadcrumb
    ///  </summary>
    public class BreadcrumbViewModel
    {
        /// <summary>
        /// Link Name
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// Link URL
        /// </summary>
        public string LInkUrl { get; set; }
    }

    public static class BreadcrumbHelper
    {
        /// <summary>  
        /// Create an HTML tree from a recursive collection of items  
        /// </summary>  
        public static Breadcrumb Breadcrumb(this HtmlHelper html, IEnumerable<BreadcrumbViewModel> items)
        {
            return new Breadcrumb(html, items);
        }
    }

    public class Breadcrumb : IHtmlString
    {

        /// <summary>
        /// List of the Breadcrumb link starting from home page and ending in the current page
        /// </summary>
        private readonly IEnumerable<BreadcrumbViewModel> _items;

        /// <summary>
        /// Private HTML helper for the purpose of use inside this class
        /// </summary>
        private readonly HtmlHelper _html;

        /// <summary>
        /// Private Varibale for Css Classes
        /// </summary>
        private string _cssClass;

        /// <summary>
        /// Add Css Classes
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Breadcrumb CssCLass(string selector)
        {
            _cssClass = selector;
            return this;
        }

        /// <summary>
        /// Private Dictionary for holding HTML attributes for the purpose of using inside this class
        /// </summary>
        private IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();

        /// <summary>
        /// Breadcrumb Default Values
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="items">Items</param>
        public Breadcrumb(HtmlHelper html, IEnumerable<BreadcrumbViewModel> items)
        {
            _html = html ?? throw new ArgumentNullException(nameof(html));
            _items = items;
        }


        /// <summary>  
        /// HTML attributes appended to the root NAV node  
        /// </summary>  
        public Breadcrumb HtmlAttributes(object htmlAttributes)
        {
            _htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return this;
        }

        /// <summary>
        /// Return the HTML string format
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString()
        {
            return ToString();
        }

        /// <summary>
        /// Render the HTML tags
        /// </summary>
        public void Render()
        {
            var writer = _html.ViewContext.Writer;
            using (var textWriter = new HtmlTextWriter(writer))
            {
                textWriter.Write(ToString());
            }
        }


        /// <summary>
        /// Generate the HTML tag as List which will be used for creating Tree View
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //Get the list that will be used for creating the HTML object
            var listItems = new List<BreadcrumbViewModel>();
            if (_items != null)
            {
                listItems = _items.ToList();
            }


            //  UL Tag that start the HTML object
            var nav = new TagBuilder("nav");

            //Add the basic Attribute relaed ot breadcrumb
            nav.Attributes.Add("aria-label", "breadcrumb");
            nav.AddCssClass(string.IsNullOrEmpty(_cssClass) ? "breadcrumb" : $"breadcrumb {_cssClass}");

            // Get the HTML attributes sent to this class and add it to our defined ul object
            nav.MergeAttributes(_htmlAttributes);

            //Check if the number of items in our list is more than 0
            if (listItems.Count > 0)
            {

                //Add a place holder
                //<a class="dropdown-item" data-value="" data-level="1" data-default-selected="" href="#">All categories</a>
                var rootOl = new TagBuilder("ol");

                //Add needed HTML Attributes
                rootOl.AddCssClass("breadcrumb");

                var itemCounter = 0;
                foreach (var item in listItems)
                {
                    //Create an li as a record
                    var li = new TagBuilder("li");

                    //Link
                    var aLink = new TagBuilder("a");

                    aLink.Attributes.Add("href", item.LInkUrl);
                    aLink.Attributes.Add("target", "_blank");
                    aLink.InnerHtml = item.LinkName;

                    if (itemCounter == listItems.Count - 1)
                    {
                        li.AddCssClass("breadcrumb-item active");
                        li.Attributes.Add("aria-current", "page");
                        li.InnerHtml = item.LinkName;
                    }
                    else
                    {
                        li.AddCssClass("breadcrumb-item");
                        li.InnerHtml = aLink.ToString();
                    }

                    rootOl.InnerHtml += $"{li}\n";

                    itemCounter++;
                }


                nav.InnerHtml = rootOl.InnerHtml;
                
            }

            //Return the Final Nav tag
            return nav.ToString();
        }
    }
}
