using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Models.View_Models
{
    public class ChatMessage
    {
        public Message Message { get; set; }

        [Display(Name = "Name")]
        public string OtherAccountContactName { get; set; } // Mark the chat with the name the user gave the contact
                
        public string LastSender { get; set; } // Either: 'You' or OtherAccountContactName

        public bool RemoveChatAvailable { get; set; } // Button invisible if you didn't send the chat
    }
}