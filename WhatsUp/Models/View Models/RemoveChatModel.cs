using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatsUp.Models.View_Models
{
    public class RemoveChatModel
    {
        [Display(Name = "Total Messages")]
        public int TotalMessages { get; set; }

        [Display(Name = "Contact")]
        public string Name { get; set; }

        public int GroupId { get; set; }
    }
}