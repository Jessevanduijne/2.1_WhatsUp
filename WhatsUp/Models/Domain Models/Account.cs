using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsUp.Models.Domain_Models
{
    public class Account
    {
        // Fields:
        [Key]
        public int AccountId { get; set; }

        [Required, Display(Name = "Mobile number")]
        public string MobileNumber { get; set; }

        [Required, Display(Name = "Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; } // Encrypt this

        // Collections:
        [InverseProperty("ContactOwner")]
        public virtual ICollection<Contact> Contacts { get; set; } // Account can have multiple contacts

        [InverseProperty("Sender")]
        public virtual ICollection<Chat> SentChats { get; set; } // Chats sent by this account

        [InverseProperty("Receiver")]
        public virtual ICollection<Chat> ReceivedChats { get; set; } // Chats received by this account

        [InverseProperty("GroupOwner")]
        public virtual ICollection<Group> OwnedGroups { get; set; } // Groups this account owns
        [InverseProperty("Accounts")]
        public virtual ICollection<Group> Groups { get; set; } // Groups this account participates in

        [InverseProperty("GroupMessageSender")]
        public virtual ICollection<GroupMessage> GroupMessagesSent { get; set; }
    }
}