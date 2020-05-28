using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsUp.Models.Domain_Models
{
    public class Contact
    {
        [Key]       
        public int ContactId { get; set; }

        // Foreign Keys:
        public int? OwnerAccountId { get; set; } // Not required
        public int ContactOwnerId { get; set; } 

        // Properties:
        [Required(ErrorMessage = "Please enter a contactname"), Display(Name = "Name")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Please enter a mobile number"), Display(Name = "Mobile number")]
        public string MobileNumber { get; set; }

        // Navigation properties:
        [ForeignKey("ContactOwnerId")]
        public virtual Account ContactOwner { get; set; } // The account that has added this contact

        [ForeignKey("OwnerAccountId")]
        public virtual Account Account { get; set; } // The account that this contact is
    }
}