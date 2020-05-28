using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Input_Models;

namespace WhatsUp.Models.View_Models
{
    public class GroupChatModel
    {
        public int GroupId { get; set; } // pass data

        // Viewmodel-part:
        [Display(Name = "Group name")]
        public string GroupName { get; set; }
        public IEnumerable<GroupMessageModel> GroupMessages { get; set; }      
        public string GroupOwner { get; set; }
        public List<string> Participants { get; set; }
        
        // Inputmodel-part:
        [Required(ErrorMessage = "Please enter a message before sending it")]
        [DataType(DataType.Text)]
        public string MessageInput { get; set; }


    }
}