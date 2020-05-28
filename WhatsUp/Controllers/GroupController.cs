using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Input_Models;
using WhatsUp.Models.Business_Logic;
using WhatsUp.Repositories;
using WhatsUp.Models.View_Models;

namespace WhatsUp.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        IGroupRepository groupRepository = new GroupRepository();
        IContactRepository contactRepository = new ContactRepository();
        IAccountRepository accountRepository = new AccountRepository();
        GroupLogic groupLogic = new GroupLogic();

        private Account GetAccount()
        {
            Account account = (Account)Session["loggedin_account"];
            return account;
        }

        [HttpGet]
        public ActionResult Index()
        {
            Account account = GetAccount();
            IEnumerable<GroupMessageModel> lastMessages = groupLogic.GetLastMessages(account.AccountId).OrderByDescending(x => x.GroupMessage.MessageSent); // Most recent message above    
            return View(lastMessages);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Account account = GetAccount();
            IEnumerable<Contact> contacts = contactRepository.GetAllContactsForAccount(account.AccountId);

            AddGroupModel model = new AddGroupModel();
            model.Contacts = contacts;

            return View(model); // Load all available contacts
        }

        [HttpPost]
        public ActionResult Add(AddGroupModel model)
        {
            Account account = GetAccount();
            model.Contacts = contactRepository.GetAllContactsForAccount(account.AccountId);

            if (ModelState.IsValid) // Values need to be filled in
            {
                if (!model.SelectedContacts.Contains(null)) // Selected contacts must all exist
                {
                    Group group = new Group(); // Needed to Redirect To Action                                

                    int?[] selectedContacts = model.SelectedContacts;
                    string groupName = model.GroupName;
                    string initialMessage = model.InitialMessage;                                   

                    // Create group, including initial message:
                    groupLogic.CreateGroup(group, selectedContacts, initialMessage, model.GroupName, account.AccountId);
                    return RedirectToAction("ViewChat", "Group", new { groupId = group.GroupId });
                }
                else ModelState.AddModelError("", "One or more of the contacts you tried to add don't have a WhatsUp-account yet.");
            }
            else ModelState.AddModelError("", "Please enter a groupname and initial message.");
            return View(model);
        }

        [HttpGet]
        public ActionResult ViewChat(int groupId)
        {
            Account account = GetAccount();
            GroupChatModel model = new GroupChatModel();
            Group group = groupRepository.GetGroup(groupId);

            model.GroupOwner = groupLogic.GetGroupOwner(account.AccountId, group);
            model.GroupName = group.GroupName;
            model.GroupMessages = groupLogic.GetGroupMessages(account.AccountId, group).OrderByDescending(x => x.GroupMessage.MessageSent); // newest first
            model.GroupId = group.GroupId;
            model.Participants = groupLogic.GetParticipants(group.Accounts, account.AccountId);

            return View(model);
        }

        [HttpPost, ActionName("ViewChat")]
        [ValidateAntiForgeryToken]
        public ActionResult AddMessage(GroupChatModel model)
        {
            Account account = GetAccount();
            Group group = groupRepository.GetGroup(model.GroupId);

            if (ModelState.IsValid) // If a message is entered
            {
                groupLogic.CreateMessage(model.MessageInput, group.GroupId, account.AccountId);
                return RedirectToAction("ViewChat", "Group", new { groupId = group.GroupId }); // Reload chat
            }
            else ModelState.AddModelError("", "Please enter a message");
            return View(model);
        }

        [HttpGet]
        public ActionResult Remove(int groupId)
        {
            Account account = GetAccount();
            Group group = groupRepository.GetGroup(groupId);
            RemoveChatModel model = new RemoveChatModel();

            model.Name = group.GroupName;
            model.TotalMessages = group.GroupMessages.Count();
            model.GroupId = group.GroupId;

            return View(model);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int groupId) 
        {
            Account account = GetAccount();
            

            Group group = groupRepository.GetGroup(groupId);
            group.Accounts.Clear(); // Remove all participants in AccountGroups (DB)

            account.Groups.Remove(group); // Remove group from groups linked to account

            groupRepository.RemoveGroup(group);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveMessage(GroupMessage message)
        {
            int rememberedGroupId = message.GroupId; // Keep groupId to reload this chat            
            groupRepository.RemoveGroupMessage(message); 

            return RedirectToAction("ViewChat", "Group", new { groupId = rememberedGroupId }); 
        }
    }
}