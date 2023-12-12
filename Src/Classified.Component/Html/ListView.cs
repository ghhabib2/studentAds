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
    public static class ListViewHelper
    {
        /// <summary>  
        /// Create an HTML tree from a recursive collection of items  
        /// </summary>  
        public static ListView<T> TreeView<T>(this HtmlHelper html, IEnumerable<T> items)
        {
            return new ListView<T>(html, items);
        }
    }

    /// <summary>
    /// Create an HTML tree from a recursive collection of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListView<T> : IHtmlString
    {
        /// <summary>
        /// Private HTML helper for the purpose of use inside this class
        /// </summary>
        private readonly HtmlHelper _html;
        /// <summary>
        /// Private IEnumerable items with the purpose of use inside this class
        /// </summary>
        private readonly IEnumerable<T> _items;
        /// <summary>
        /// Private Function for displaying properties of our component for the purpose of use inside this class
        /// </summary>
        private Func<T, string> _displayProperty = item => item.ToString();

        /// <summary>
        /// Private Function for children properties for the purpose of use inside this class
        /// </summary>
        private Func<T, IEnumerable<T>> _childrenProperty;

        /// <summary>
        /// Use for showing The items with no children
        /// </summary>
        private string _emptyContent = "No children";

        /// <summary>
        /// Private Dictionary for holding html attributes for the purpose of using inside this class
        /// </summary>
        private IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();

        /// <summary>
        /// Private Dictionary for holding html attributes of child Html items for the purpose of using inside this class
        /// </summary>
        private IDictionary<string, object> _childHtmlAttributes = new Dictionary<string, object>();

        /// <summary>
        /// Private function for holding item template for the purpose of using inside this class.
        /// </summary>
        private Func<T, HelperResult> _itemTemplate;

        /// <summary>
        /// Tree View with HTML Helper and Items to display.
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="items">Items</param>
        public ListView(HtmlHelper html, IEnumerable<T> items)
        {
            _html = html ?? throw new ArgumentNullException(nameof(html));
            _items = items;
            // The ItemTemplate will default to rendering the DisplayProperty  
            _itemTemplate = item => new HelperResult(writer => writer.Write(_displayProperty(item)));
        }

        /// <summary>  
        /// The property which will display the text rendered for each item  
        /// </summary>  
        public ListView<T> ItemText(Func<T, string> selector)
        {
            _displayProperty = selector ?? throw new ArgumentNullException(nameof(selector));
            return this;
        }

        /// <summary>  
        /// The template used to render each item in the tree view  
        /// </summary>  
        public ListView<T> ItemTemplate(Func<T, HelperResult> itemTemplate)
        {
            _itemTemplate = itemTemplate ?? throw new ArgumentNullException(nameof(itemTemplate));
            return this;
        }

        /// <summary>  
        /// The property which returns the children items  
        /// </summary>  
        public ListView<T> Children(Func<T, IEnumerable<T>> selector)
        {
            //  if (selector == null) //throw new ArgumentNullException("selector");  
            _childrenProperty = selector;
            return this;
        }

        /// <summary>  
        /// Content displayed if the list is empty  
        /// </summary>  
        public ListView<T> EmptyContent(string emptyContent)
        {
            _emptyContent = emptyContent ?? throw new ArgumentNullException(nameof(emptyContent));
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the root ul node  
        /// </summary>  
        public ListView<T> HtmlAttributes(object htmlAttributes)
        {
            _htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the root ul node  
        /// </summary>  
        public ListView<T> HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            _htmlAttributes = htmlAttributes ?? throw new ArgumentNullException(nameof(htmlAttributes));
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the children items  
        /// </summary>  
        public ListView<T> ChildrenHtmlAttributes(object htmlAttributes)
        {
            ChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>  
        /// HTML attributes appended to the children items  
        /// </summary>  
        public ListView<T> ChildrenHtmlAttributes(IDictionary<string, object> htmlAttributes)
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
            var ul = new TagBuilder("ul");
            // Get the HTML attributes sent to this class and add it to our defined ul object
            ul.MergeAttributes(_htmlAttributes);

            //Add the very fist LI tag with no Contnet
            var li = new TagBuilder("li")
            {
                InnerHtml = _emptyContent
            };
            // Add an attribute with the name of id and value of -1
            li.MergeAttribute("id", "-1");

            //Check if the number of items in our list is more than 0
            if (listItems.Count > 0)
            {
                // Define a inner ul tag for children of each items
                var innerUl = new TagBuilder("ul");
                //Add the child html attributes to our defined inner ul tag
                innerUl.MergeAttributes(_childHtmlAttributes);

                //Surf the list for 
                foreach (var item in listItems)
                {
                    //Build the li tags for the defined ul
                    BuildNestedTag(innerUl, item, _childrenProperty);
                }
                // add the inner defined Ul to our Li that defined at first place which showing our root.
                li.InnerHtml += innerUl.ToString();
            }
            // Finalize creating our UL-LI by adding the creating and completed root to our defined LI Tag
            ul.InnerHtml += li.ToString();

            //Return the final results
            return ul.ToString();
        }

        /// <summary>
        /// Build Nested Tag based on the values of List provided for Tree View
        /// </summary>
        /// <param name="parentTag">Parent Tag</param>
        /// <param name="parentItem">parent Item</param>
        /// <param name="childrenProperty">Children Property</param>
        private void BuildNestedTag(TagBuilder parentTag, T parentItem, Func<T, IEnumerable<T>> childrenProperty)
        {
            // define the Li tag for children tags
            var li = GetLi(parentItem);
            // Add the li tag
            parentTag.InnerHtml += li.ToString(TagRenderMode.StartTag);
            // Append the children to difined Li tag
            AppendChildren(li, parentItem, childrenProperty);
            // Add the created children LI tag to the partent UI tag.
            parentTag.InnerHtml += li.InnerHtml + li.ToString(TagRenderMode.EndTag);
        }

        /// <summary>
        /// Append Children
        /// </summary>
        /// <param name="parentTag">Parent Tag</param>
        /// <param name="parentItem">Parent Item</param>
        /// <param name="childrenProperty">Children Property</param>
        private void AppendChildren(TagBuilder parentTag, T parentItem, Func<T, IEnumerable<T>> childrenProperty)
        {
            //  check if there is any property for children
            if (childrenProperty == null)
            {
                return;
            }
            // Convert the children items to list 
            var children = childrenProperty(parentItem).ToList();
            if (!children.Any())
            {
                return;
            }

            //Create a UL tag for adding the children
            var innerUl = new TagBuilder("ul");
            // Add the children html attributes to the defined UL tag
            innerUl.MergeAttributes(_childHtmlAttributes);

            //surf the child list
            foreach (var item in children)
            {
                //Add the child to defined child list if there is any
                BuildNestedTag(innerUl, item, childrenProperty);
            }
            //Finalize the creation of the HTML UL child tag 
            parentTag.InnerHtml += innerUl.ToString();
        }

        /// <summary>
        /// Create the Li Tags
        /// </summary>
        /// <param name="item">Item to be converted to Li tag</param>
        /// <returns>Return the Li Tag</returns>
        private TagBuilder GetLi(T item)
        {
            //Define the LI tag
            var li = new TagBuilder("li")
            {
                // Add the HTML template to Inner HTML of li
                InnerHtml = _itemTemplate(item).ToHtmlString()
            };

            // Get the type of Item.
            Type myType = item.GetType();

            // Convert the properties of item in to a list
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            //  Surf the properties
            foreach (var prop in props)
            {
                //check the id
                if (prop.Name.ToLower() == "id")
                    // add the id to LI
                    li.MergeAttribute("id", prop.GetValue(item, null).ToString());
                // Do something with propValue  
                if (prop.Name.ToLower() == "sortorder")
                    // Add the Name to the Tree
                    li.MergeAttribute("priority", prop.GetValue(item, null).ToString());
            }
            // Return the LI tag
            return li;
        }

    }

}

