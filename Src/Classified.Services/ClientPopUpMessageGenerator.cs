using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Classified.Domain.ViewModels;

namespace Classified.Services
{
    /// <summary>
    /// Manage the Messages the would be displayed in client side
    /// </summary>
    public static class PopUpMessageGenerator
    {
       
        /// <summary>
        /// Generate the message that to be displayed in output  form
        /// </summary>
        /// <param name="title">Title of the Message</param>
        /// <param name="message">Text of the Message</param>
        /// <param name="messageStatus">Status of the Message</param>
        public static void GenerateMessage(string title, string message, MessageStatus messageStatus)
        {
            HttpContext.Current.Session["popUpMessage"] = new MessageViewModle
            {
                MessageTitle = title,
                MessageText = message,
                MessageStatus = messageStatus
            };
        }

        /// <summary>
        /// Generate the Script that is going to display the message
        /// </summary>
        public static string GenerateMessageOutPut
        {
            get
            {
                if (HttpContext.Current.Session["popUpMessage"] != null)
                {
                    //Read the Message
                    var tempMessage = (MessageViewModle)HttpContext.Current.Session["popUpMessage"];

                    //Remove Session
                    HttpContext.Current.Session.Remove("popUpMessage");

                    //Define Style
                    var tempStyle = string.Empty;

                    switch (tempMessage.MessageStatus)
                    {
                        case MessageStatus.Successfull:
                            {
                                tempStyle = MessageStyle.Success;
                                break;
                            }
                        case MessageStatus.Failed:
                            {
                                tempStyle = MessageStyle.Failed;
                                break;
                            }
                        case MessageStatus.Warning:
                            {
                                tempStyle = MessageStyle.Warning;
                                break;
                            }
                    }

                    //Generate the Message Text
                    return string.Format("<script>bootbox.alert({{className:'{0}' ,title: '{1}', message: '{2}' }});</script>",
                        tempStyle, tempMessage.MessageTitle, tempMessage.MessageText);
                }
                else
                {
                    return string.Empty;
                }
            }

        }
    }
}
