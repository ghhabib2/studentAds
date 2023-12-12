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

    public static class WYSIWYGEditorHelper{

        /// <summary>
        /// Editor Without Value
        /// </summary>
        /// <param name="name">Name of the Control</param>
        /// <param name="state">State of the Control</param>
        /// <param name="html">current HTML helper</param>
        /// <returns>Ready to render control</returns>
        public static WYSIWYGEditor WYSIWYGEditor(this HtmlHelper html, string name,EditorState state)
        {
            return new WYSIWYGEditor(html)
            {
                Name = name,
                EditorState = state,
                Value = string.Empty
            };
        }

        /// <summary>
        /// Editor With Value
        /// </summary>
        /// <param name="name">Name of the Control</param>
        /// <param name="state">State of the control</param>
        /// <param name="value">Value of the control</param>
        /// <param name="html">Current HTML helper</param>
        /// <returns>Ready to render control</returns>
        public static WYSIWYGEditor WYSIWYGEditor(this HtmlHelper html, string name, EditorState state,string value)
        {
            return new WYSIWYGEditor(html)
            {
                Name = name,
                EditorState = state,
                Value = value
            };
        }
    }


    public enum EditorState
    {
        BasicEditor=1
    }

    /// <summary>
    /// Helper for creation of WYSIWYG Editor
    /// </summary>
    public class WYSIWYGEditor : IHtmlString
    {

        /// <summary>
        /// Private HTML helper for the purpose of use inside this class
        /// </summary>
        private readonly HtmlHelper _html;

        public WYSIWYGEditor(HtmlHelper html)
        {
            _html = html ?? throw new ArgumentNullException(nameof(html));
        }

        /// <summary>
        /// Private Variable for Keeping the Name and Id of the control
        /// </summary>
        private string _editorName;

        /// <summary>
        /// String value of the editor
        /// </summary>
        private string _editorValue;

        /// <summary>
        /// State of the Editor
        /// </summary>
        private EditorState _editorState;

        /// <summary>
        /// Name of the Control
        /// </summary>
        public string Name
        {
            set
            {
                _editorName=value;
            }
        }

        /// <summary>
        /// Value of the Editor
        /// </summary>
        [AllowHtml]
        public string Value
        {
            set { _editorValue = value; }
        }

        /// <summary>
        /// State of the Editor
        /// </summary>
        public EditorState EditorState
        {
            set { _editorState = value; }
        }

        /// <summary>
        /// Private Dictionary for holding html attributes for the purpose of using inside this class
        /// </summary>
        private IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();

        /// <summary>  
        /// HTML attributes appended to the root ul node  
        /// </summary>  
        public WYSIWYGEditor HtmlAttributes(object htmlAttributes)
        {
            HtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        private string _placeHolder = string.Empty;

        /// <summary>
        /// Set the Place Holder
        /// </summary>
        /// <param name="placeHolder">Place holder text</param>
        /// <returns></returns>
        public WYSIWYGEditor PlaceHolder(string placeHolder)
        {
            _placeHolder = placeHolder;
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
        /// Create the Control
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //Editor object
            var editor = new TagBuilder("textarea");

            // Final HTML string
            var htmlString = string.Empty;

            //Add the HTML Attributes
            editor.MergeAttributes(_htmlAttributes);

            editor.Attributes.Add("id",_editorName);
            editor.Attributes.Add("name", _editorName);
            

            if (!string.IsNullOrEmpty(_editorValue))
                editor.InnerHtml = _editorValue;

            //Add Editors
            htmlString = $"{editor}\n";

            //Add script part
            htmlString += ScriptGenerator(_editorState);

            //Return Final Results
            return htmlString;
        }

        /// <summary>
        /// Return a script object based on the editor state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string ScriptGenerator(EditorState state)
        {
            var scriptBuilder = new TagBuilder("Script");

            scriptBuilder.MergeAttribute("type", "text/javascript");

            //Start the scripting in jquery
            scriptBuilder.InnerHtml = "\n$(document).ready(function(){\n";

            //Start Adding the editor related codes 
            scriptBuilder.InnerHtml += string.Format("\n$(\"#{0}\").summernote({{\n", _editorName);
            scriptBuilder.InnerHtml += ReturnStateToolBox(state);

            //Adding Place Holder
            if (!string.IsNullOrEmpty(_placeHolder))
            {
                scriptBuilder.InnerHtml += ",\n";
                scriptBuilder.InnerHtml += ReturnPlaceHolder();
            }
            scriptBuilder.InnerHtml += "\n});";

            //Close the over-all script
            scriptBuilder.InnerHtml += "\n});\n";

            return scriptBuilder.ToString();
        }

        /// <summary>
        /// Return the script of the place Holder
        /// </summary>
        /// <returns></returns>
        private string ReturnPlaceHolder()
        {
            return $"placeholder: '{_placeHolder}'";
        }

        /// <summary>
        /// Return State related String
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string ReturnStateToolBox(EditorState state)
        {
            switch (state)
            {
                case EditorState.BasicEditor:
                {
                    return @"toolbar: [
                                                // [groupName, [list of button]]
                                                ['style', ['style','bold', 'italic', 'underline', 'clear']],
                                                ['font', ['strikethrough', 'superscript', 'subscript']],
                                                ['fontsize', ['fontsize']],
                                                ['color', ['color']],
                                                ['para', ['ul', 'ol', 'paragraph']],
                                                ['height', ['height']]
                                            ]";
                 }
            }

            return string.Empty;
        }
    }
}

