using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatsUp.Models.Input_Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Mobile number is required")]
        [DataType(DataType.PhoneNumber)]
        [MinLength(8, ErrorMessage = "Mobile Number doesn't have the right length")]
        [MaxLength(12, ErrorMessage = "Mobile Number doesn't have the right length")]
        [RegularExpression("^[0-9 ]*$", ErrorMessage = "Your phone number can only contain digits or spaces")]
        public string MobileNumber { get; set; }

        [MinLength(2, ErrorMessage = "First name should include at least 2 characters")]
        [MaxLength(12, ErrorMessage = "First name can contain max. 12 characters")]
        [Required(ErrorMessage = "First name is required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "Last name should include at least 2 characters")]
        [MaxLength(20, ErrorMessage = "Last name can contain max. 20 characters")]
        [Required(ErrorMessage = "Last name is required required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [MinLength(5, ErrorMessage = "Your password should contain at least 5 characters")]
        [MaxLength(20, ErrorMessage = "Your password can contain max. 20 characters")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}