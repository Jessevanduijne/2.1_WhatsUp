using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Models.Input_Models
{
    public class AddChatModel
    {
        public int? SelectedContact { get; set; } // Refers to OwnerAccountId of Contact, which can be null
        public IEnumerable<Contact> Contacts { get; set; }

        [Required(ErrorMessage = "Please enter a message before sending it")]
        [DataType(DataType.Text)]
        public string InitialMessage { get; set; }

    }
}