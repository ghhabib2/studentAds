using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;

namespace Classified.Component.Html
{

    
    /// <summary>
    /// Create the Tree View
    /// </summary>
    public static class URLViewHelper
    {
        /// <summary>  
        /// Create an HTML tree from a recursive collection of items  
        /// </summary>  
        public static URLView<T> URLView<T>(this HtmlHelper html, IEnumerable<T> items)
        {
            return new URLView<T>(html, items);
        }
    }

    /// <summary>
    /// Create an HTML tree from a recursive collection of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class URLView<T> : IHtmlString
    {

        /// <summary>
        /// Selected Value of the Drop Down List
        /// </summary>
        private string _selectedValue;

        /// <summary>
        /// Private HTML helper for the purpose of use inside this class
        /// </summary>
        private readonly HtmlHelper _html;
        /// <summary>
        /// Private IEnumerable items with the purpose of use inside this class
        /// </summary>
        private readonly IEnumerable<T> _items;


        /// <summary>
        /// The Name of the Item that should be displayed in drop-down list
        /// </summary>
        private string _displayName;

        private string _displayValue;

        private string _displayUrl;

        /// <summary>
        /// Private Function for children properties for the purpose of use inside this class
        /// </summary>
        private Func<T, IEnumerable<T>> _childrenProperty;

        /// <summary>
        /// Use for showing The items with no children
        /// </summary>
        private string _placeHolder = "No children";

        /// <summary>
        /// Private Dictionary for holding HTML attributes for the purpose of using inside this class
        /// </summary>
        private IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();

        /// <summary>
        /// Private Dictionary for holding HTML attributes of child HTML items for the purpose of using inside this class
        /// </summary>
        private IDictionary<string, object> _childHtmlAttributes = new Dictionary<string, object>();



        /// <summary>
        /// Selected Value of the Drop DownList
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public URLView<T> SelectedValue(string selector)
        {
            _selectedValue = selector;
            return this;
        }

        /// <summary>`
        /// Tree View with HTML Helper and Items to display.
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="items">Items</param>
        public URLView(HtmlHelper html, IEnumerable<T> items)
        {
            _html = html ?? throw new ArgumentNullException(nameof(html));
            _items = items;

        }

        /// <summary>  
        /// The property which will display the text rendered for each item  
        /// </summary>  
        public URLView<T> ItemText(string selector)
        {
            _displayName = selector;
            return this;
        }

        /// <summary>  
        /// The property which will display the text rendered for each item  
        /// </summary>  
        public URLView<T> ItemValue(string selector)
        {
            _displayValue = selector;
            return this;
        }

        /// <summary>  
        /// The property which will display the text rendered for each item  
        /// </summary>  
        public URLView<T> ItemURL(string selector)
        {
            _displayUrl = selector;
            return this;
        }


        /// <summary>  
        /// The property which returns the children items  
        /// </summary>  
        public URLView<T> Children(Func<T, IEnumerable<T>> selector)
        {
            //  if (selector == null) //throw new ArgumentNullException("selector");  
            _childrenProperty = selector;
            return this;
        }

        /// <summary>  
        /// Content displayed if the list is empty  
        /// </summary>  
        public URLView<T> PlaceHolder(string placeHolderText)
        {
            _placeHolder = placeHolderText ?? throw new ArgumentNullException(nameof(placeHolderText));
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the root ul node  
        /// </summary>  
        public URLView<T> HtmlAttributes(object htmlAttributes)
        {
            _htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the root ul node  
        /// </summary>  
        public URLView<T> HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            _htmlAttributes = htmlAttributes ?? throw new ArgumentNullException(nameof(htmlAttributes));
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the children items  
        /// </summary>  
        public URLView<T> ChildrenHtmlAttributes(object htmlAttributes)
        {
            ChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the children items  
        /// </summary>  
        public URLView<T> ChildrenHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            _childHtmlAttributes = htmlAttributes ?? throw new ArgumentNullException(nameof(htmlAttributes));
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
        /// Validate the settings of component
        /// </summary>
        private void ValidateSettings()
        {
            if (_childrenProperty == null)
            {
                return;
            }
        }

        /// <summary>
        /// Generate the HTML tag as List which will be used for creating Tree View
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //Validate the Settings
            ValidateSettings();

            //Get the list that will be used for creating the HTML object
            var listItems = new List<T>();
            if (_items != null)
            {
                listItems = _items.ToList();
            }

            //  UL Tag that start the HTML object
            var div = new TagBuilder("div");
            // Get the HTML attributes sent to this class and add it to our defined ul object
            div.MergeAttributes(_htmlAttributes);

            //Check if the number of items in our list is more than 0
            if (listItems.Count > 0)
            {

                var dataLevel = 1;
                //Surf the list for 
                foreach (var item in listItems)
                {

                    // Define a link tag for children of each items
                    var a = GetA();

                    //Get the type of List items
                    Type type = item.GetType();

                    var itemText = type.GetProperty(_displayName);
                    var itemValue = type.GetProperty(_displayValue);
                    var itemURL = type.GetProperty(_displayUrl);

                    a.MergeAttributes(_childHtmlAttributes);
                    a.Attributes.Add("class", "dropdown-item");
                    a.Attributes.Add("data-level", dataLevel.ToString());
                    a.Attributes.Add("data-value", itemValue.GetValue(item, null).ToString());
                    a.Attributes.Add("href", itemURL.GetValue(item, null).ToString());

                    if (string.Equals(_selectedValue, itemValue.GetValue(item, null).ToString()))
                    {
                        a.Attributes.Add("data-default-selected", "");
                    }

                    //List of Child nodes if there any
                    var tempChild = _childrenProperty(item).ToList();

                    if (tempChild.Any())
                    {

                        a.InnerHtml = $"<b>{itemText.GetValue(item, null)}</b>";

                        div.InnerHtml += $"{a}\n";

                        //Add the child link
                        BuildNestedTag(ref div, tempChild, dataLevel);

                     }
                    else
                    {
                        a.InnerHtml = $"{itemText.GetValue(item, null)}";

                        div.InnerHtml += $"{a}\n";
                    }
                }

            }

            //Return the final results
            return div.ToString();
        }

        /// <summary>
        /// Build Nested Tag based on the values of List provided for Tree View
        /// </summary>
        /// <param name="targetParentObject">Parent Tag</param>
        /// <param name="childrenProperty">Children Property</param>
        private void BuildNestedTag(ref TagBuilder targetParentObject, IEnumerable<T> childrenProperty, int currentLevel)
        {

            foreach (var item in childrenProperty)
            {
                // Define a link tag for children of each items
                var a = GetA();

                var childLevel = currentLevel + 1;

                //Get the type of List items
                Type type = item.GetType();

                var itemText = type.GetProperty(_displayName);
                var itemValue = type.GetProperty(_displayValue);
                var itemURL = type.GetProperty(_displayUrl);

                a.MergeAttributes(_childHtmlAttributes);
                a.Attributes.Add("class", "dropdown-item");
                a.Attributes.Add("data-level", childLevel.ToString());
                a.Attributes.Add("data-value", itemValue.GetValue(item, null).ToString());
                a.Attributes.Add("href",itemURL.GetValue(item,null).ToString());

               if (string.Equals(_selectedValue, itemValue.GetValue(item, null).ToString()))
                {
                    a.Attributes.Add("data-default-selected", "");
                }
               
                var tempChild = _childrenProperty(item).ToList();

                if (tempChild.Any())
                {
                    a.InnerHtml = $"<b>{itemText.GetValue(item, null)}</b>";

                    targetParentObject.InnerHtml += $"{a}\n";

                    //Add the child link
                    BuildNestedTag(ref targetParentObject, tempChild, childLevel);
                }
                else
                {
                    a.InnerHtml = $"{itemText.GetValue(item, null)}";
                    targetParentObject.InnerHtml += $"{a}\n";
                }
            }
            
        }

        /// <summary>
        /// Create the Li Tags
        /// </summary>
        /// <param name="item">Item to be converted to Li tag</param>
        /// <returns>Return the Li Tag</returns>
        private TagBuilder GetA()
        {
            // Return the LI tag
            return new TagBuilder("a");
        }

    }

}

