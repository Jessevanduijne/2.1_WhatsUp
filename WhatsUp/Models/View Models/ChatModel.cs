using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Input_Models;

namespace WhatsUp.Models.View_Models
{
    public class ChatModel
    {
        // Viewmodel-part:    
        [Display(Name = "Contact")]
        public string ContactName { get; set; }
        public IEnumerable<ChatMessage> MessageList { get; set; }


        // Inputmodel-part:
        [Required(ErrorMessage = "Please enter a message before sending it")]
        [DataType(DataType.Text)]
        public string MessageInput { get; set; }
                
        public int ChatId { get; set; } // Pass data 

        public Message message { get; set; }
    }
}