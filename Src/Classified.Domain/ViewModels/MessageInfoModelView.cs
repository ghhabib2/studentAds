using System.Collections.Generic;
using System.Web.Management;
using Classified.Domain.Entities;

namespace Classified.Domain.ViewModels
{
    /// <summary>
    /// Model View for Message Infos
    /// </summary>
    public class MessageInfoModelView
    {
        /// <summary>
        /// Hold the information of Message Info List
        /// </summary>
        public IEnumerable<MessageInfo> MessageInfos { get; set; }
    }

    /// <summary>
    /// Styles Fixes Name for Messages
    /// </summary>
    public class MessageStyle
    {
        /// <summary>
        /// Success Message Style
        /// </summary>
        public const string Success = "modalSucess";
        /// <summary>
        /// Failed Message Style
        /// </summary>
        public const string Failed = "modalFailed";
        /// <summary>
        /// Warning Message Style
        /// </summary>
        public const string Warning = "modalWarning";
    }
}
