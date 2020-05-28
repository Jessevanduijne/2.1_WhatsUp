using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsUp.Models.Domain_Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [ForeignKey("GroupOwner")]
        public int GroupOwnerAccountId { get; set; }

        [Required, Display(Name = "Group Name")]
        public string GroupName { get; set; }
        public virtual List<GroupMessage> GroupMessages { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual Account GroupOwner { get; set; }
    }
}