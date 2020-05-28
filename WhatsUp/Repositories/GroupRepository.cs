using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Database_Connection;

namespace WhatsUp.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        WhatsUpContext db = new WhatsUpContext();

        public IEnumerable<Group> GetAllGroups(int accountId)
        {
            IEnumerable<Group> groups = db.Groups.Where(g => g.Accounts.Any(a => a.AccountId == accountId));
            return groups;
        }

        public void AddGroup(Group group)
        {
            db.Groups.Add(group);
            db.SaveChanges();
        }

        public void AddMessage(GroupMessage message)
        {
            db.GroupMessages.Add(message);
            db.SaveChanges();
        }

        public void AddGroupAccounts(Group group, Account account)
        {
            db.Groups.Attach(group);
            db.Accounts.Attach(account);
            db.SaveChanges();
        }

        public Account GetAccount(int accountId) 
        {
            Account account = db.Accounts.SingleOrDefault(x => x.AccountId == accountId);
            return account;
            // This can't be in the accountRepo because if so, two contexts will be mixed up when adding a group
        }

        public Group GetGroup(int groupId)
        {
            Group group = db.Groups.Find(groupId);
            return group;
        }

        public void RemoveGroup(Group group)
        {
            db.Groups.Remove(group);
            db.SaveChanges();
        }

        public GroupMessage GetGroupMessage(int groupMessageId)
        {
            GroupMessage groupMessage = db.GroupMessages.Find(groupMessageId);
            return groupMessage;
        }

        public void RemoveGroupMessage(GroupMessage message)
        {
            db.GroupMessages.Remove(message);
            db.SaveChanges();
        }

        public Contact GetGroupOwnerContact(int groupOwnerId, int accountId)
        {   // Checks if your contacts contain the groupowner, so either the name or the phone number will be displayed
            Contact contact = db.Contacts.SingleOrDefault(x => x.OwnerAccountId == groupOwnerId && x.ContactOwnerId == accountId);
            return contact;
        }
    }
}