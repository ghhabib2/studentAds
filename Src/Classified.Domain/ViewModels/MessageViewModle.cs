using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classified.Domain.ViewModels
{

    /// <summary>
    /// Enum for Message Status
    /// </summary>
    public enum MessageStatus
    {
        Successfull = 1,
        Failed = 2,
        Warning = 3
    }

    /// <summary>
    /// Message View Model for displaying system Messages
    /// </summary>
    public class MessageViewModle
    {
        /// <summary>
        /// Message Status 
        /// </summary>
        public MessageStatus MessageStatus { get; set; }

        /// <summary>
        /// Message Title
        /// </summary>
        public string MessageTitle { get; set; }

        /// <summary>
        /// Message Text
        /// </summary>
        public string MessageText { get; set; }
        
    }
}
