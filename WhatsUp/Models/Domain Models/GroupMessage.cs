using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsUp.Models.Domain_Models
{
    public class GroupMessage
    {
        [Key]
        public int GroupMessageId { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }

        [ForeignKey("GroupMessageSender")]
        public int SenderId { get; set; }

        [Display(Name = "Message")]
        public string TextMessage { get; set; }

        [Display(Name = "Time")]
        public DateTime MessageSent { get; set; }

        public virtual Group Group { get; set; }
        public virtual Account GroupMessageSender { get; set; }
    }
}