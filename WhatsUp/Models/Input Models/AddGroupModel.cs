using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Models.Input_Models
{
    public class AddGroupModel
    {
        public int?[] SelectedContacts { get; set; } // Refers to OwnerAccountId of Contact, which can be null

        [MaxLength(20, ErrorMessage = "Your groupname can contain max. 20 characters")]
        [Required(ErrorMessage = "You'll need a groupname if you want to create a group")]
        public string GroupName { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }

        [Required(ErrorMessage = "Please enter a message before sending it")]
        [DataType(DataType.Text)]
        public string InitialMessage { get; set; }
    }
}