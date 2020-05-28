using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Models.View_Models
{
    public class GroupMessageModel
    {
        [Display(Name = "Name")]
        public string Sender { get; set; }

        public GroupMessage GroupMessage { get; set; }

        public bool RemoveChatAvailable { get; set; } // Button invisible if you didn't send the chat
    }
}