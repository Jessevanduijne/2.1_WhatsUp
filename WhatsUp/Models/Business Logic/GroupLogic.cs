using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsUp.Repositories;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Input_Models;
using WhatsUp.Models.View_Models;

namespace WhatsUp.Models.Business_Logic
{
    public class GroupLogic
    {
        IGroupRepository groupRepository = new GroupRepository();
        IContactRepository contactRepository = new ContactRepository();
        IAccountRepository accountRepository = new AccountRepository();

        public void CreateGroup(Group group, int?[] selectedContacts, string initialMessage, string groupName, int accountId)
        {
            group.GroupOwnerAccountId = accountId;
            group.GroupName = groupName;
            group.Accounts = new List<Account>();
            groupRepository.AddGroup(group);

            // Because of the different context classes, account is retrieved again in the groupRepository
            Account yourAccount = groupRepository.GetAccount(accountId); 
            LinkAccountWithGroup(yourAccount, group);

            foreach (int? selectedContact in selectedContacts) // ..and add all the selected contacts
            {
                Account selectedAccount = groupRepository.GetAccount((int)selectedContact);
                LinkAccountWithGroup(selectedAccount, group);
            }

            CreateMessage(initialMessage, group.GroupId, accountId); // Initial message
        }

        public void CreateMessage(string message, int groupId, int accountId)
        {
            GroupMessage groupMessage = new GroupMessage();
            groupMessage.MessageSent = DateTime.Now;
            groupMessage.TextMessage = message;
            groupMessage.GroupId = groupId;
            groupMessage.SenderId = accountId;
            
            groupRepository.AddMessage(groupMessage);
        }

        private void LinkAccountWithGroup(Account account, Group group)
        {
            group.Accounts.Add(account);
            account.Groups.Add(group);
            groupRepository.AddGroupAccounts(group, account);
        }

        public IEnumerable<GroupMessageModel> GetLastMessages(int accountId)
        {
            List<GroupMessageModel> lastMessages = new List<GroupMessageModel>();
            IEnumerable<Group> allGroups = groupRepository.GetAllGroups(accountId);

            foreach(Group g in allGroups)
            {
                GroupMessage latestMessage = g.GroupMessages.OrderByDescending(m => m.MessageSent).FirstOrDefault();
                GroupMessageModel newMessage = new GroupMessageModel();
                newMessage.GroupMessage = latestMessage;

                if (latestMessage.SenderId == accountId) newMessage.Sender = "You";
                else newMessage.Sender = CheckContactList(accountId, latestMessage);

                lastMessages.Add(newMessage);
            }
            return lastMessages;
        }

        private string CheckContactList(int accountId, GroupMessage latestMessage)
        {
            Account account = accountRepository.GetAccount(accountId);
            Contact contact = contactRepository.GetContact(accountId, latestMessage.SenderId);
            account.Contacts = contactRepository.GetAllContactsForAccount(account.AccountId);
            string unknownContactMobileNumber = accountRepository.GetAccount(latestMessage.SenderId).MobileNumber;

            if (contact != null)
            {
                if (account.Contacts.Contains(contact)) return contact.ContactName;                
                else return unknownContactMobileNumber;
            }
            else return unknownContactMobileNumber;
        }

        public List<GroupMessageModel> GetGroupMessages(int accountId, Group group)
        {
            List<GroupMessageModel> messages = new List<GroupMessageModel>();
            foreach(GroupMessage gm in group.GroupMessages)
            {
                GroupMessageModel message = new GroupMessageModel();
                message.GroupMessage = gm;                

                if (gm.SenderId == accountId) // Set sender & ability to delete message
                {
                    message.Sender = "You";
                    message.RemoveChatAvailable = true; // You can only detele messages which you posted
                }                   
                else
                {
                    message.Sender = CheckContactList(accountId, gm);
                    message.RemoveChatAvailable = false; 
                }                  

                messages.Add(message);
            }
            return messages;
        }

        public string GetGroupOwner(int accountId, Group group)
        {
            string groupOwner = "";

            if(accountId == group.GroupOwnerAccountId)
            {
                groupOwner = "You";
            }
            else
            {
                Contact contact = groupRepository.GetGroupOwnerContact(group.GroupOwnerAccountId, accountId);
                if(contact != null)
                {
                    groupOwner = contact.ContactName;
                }
                else
                {
                    groupOwner = group.GroupOwner.MobileNumber;
                }
            }
            return groupOwner;
        }

        // Participants can be either shown as the phone number of an account (unknown) or the contactname of a contact (known):
        public List<string> GetParticipants(IEnumerable<Account> accounts, int accountId) 
        {            
            List<string> participants = new List<string>();
            foreach(Account a in accounts)
            {
                if(a.AccountId != accountId) // You shall be added last
                {
                    string participant = "";

                    Contact contact = contactRepository.GetContact(a.MobileNumber, accountId);
                    if (contact == null) participant = a.MobileNumber; // Add a number if it's not in your contactList
                    else participant = contact.ContactName; // Add a contact if it's in your contactList

                    participants.Add(participant + ", "); // Comma for formatting    
                }                
            }
            participants.Add("You"); 
            return participants;
        }
    }
}