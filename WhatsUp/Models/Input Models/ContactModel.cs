using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Models.Input_Models
{
    public class ContactModel
    {
        [DataType(DataType.PhoneNumber)]
        [MinLength(8, ErrorMessage = "Mobile Number doesn't have the right length")]
        [MaxLength(12, ErrorMessage = "Mobile Number doesn't have the right length")]
        [RegularExpression("^[0-9 ]*$", ErrorMessage = "Your phone number can only contain digits or spaces")]
        [Required(ErrorMessage = "Mobile number is required")]
        [Display(Name = "Number")]
        public string MobileNumber { get; set; }

        [MaxLength(25, ErrorMessage = "Max. 25 characters allowed")]
        [Required(ErrorMessage = "Give your contact a name")]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string ContactName { get; set; }

        public int ContactId { get; set; }
    }
}