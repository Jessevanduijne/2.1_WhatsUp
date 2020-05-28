using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.Input_Models;
using WhatsUp.Repositories;
using WhatsUp.Models.View_Models;
using WhatsUp.Models.Business_Logic;

namespace WhatsUp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        IChatRepository chatRepository = new ChatRepository();
        IContactRepository contactRepository = new ContactRepository();
        ChatLogic chatLogic = new ChatLogic();

        private Account GetAccount()
        {
            Account account = (Account)Session["loggedin_account"];
            return account;
        }

        [HttpGet]
        public ActionResult Index()
        {
            Account account = GetAccount();
            IEnumerable<ChatMessage> lastAccountMessages = chatLogic.GetLastMessages(account.AccountId).OrderByDescending(x => x.Message.MessageSent); // Most recent message above  
            return View(lastAccountMessages);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Account account = GetAccount();
            IEnumerable<Contact> contacts = contactRepository.GetAllContactsForAccount(account.AccountId);

            // All available contacts to chat with are loaded in a dropdownlist:
            AddChatModel model = new AddChatModel();
            model.Contacts = contacts;

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AddChatModel model)
        {
            Account account = GetAccount();
            model.Contacts = contactRepository.GetAllContactsForAccount(account.AccountId);

            if (ModelState.IsValid) // Check for model errors
            {
                if (model.SelectedContact.HasValue) // Check if the selected contact has an account (otherwise you can't chat)
                {
                    int receiverId = (int)model.SelectedContact;
                    int senderId = account.AccountId;

                    if (!chatRepository.CheckForDuplicateChats(receiverId, senderId)) // Check if the chat doesn't exist yet (no duplicate chats)
                    {
                        Chat chat = new Chat();
                        chat.ReceiverId = receiverId;
                        chat.SenderId = senderId;

                        chatRepository.AddChat(chat); // Add chat to DB
                        chatLogic.AddMessage(model.InitialMessage, senderId, chat); // Add initial message

                        return RedirectToAction("ViewChat", "Chat", new { chatId = chat.ChatId } );
                    }
                    else ModelState.AddModelError("", "You already have a chat with this contact. Try chatting in your existing chatroom.");
                }
                else ModelState.AddModelError("", "The selected contact doesn't have an account yet. Try another contact!");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ViewChat(int chatId)
        {
            Account account = GetAccount();
            Chat chat = chatRepository.GetChat(chatId);
            ChatModel model = new ChatModel(); // Create presentation model (contactname + messagelist + message)
                        
            model.MessageList = chatLogic.GetAllMessages(chat, account.AccountId).OrderByDescending(x => x.Message.MessageSent);
            model.ContactName = chatLogic.GetContactName(chat, account.AccountId);
            model.ChatId = chatId;
            model.message = model.MessageList.First().Message;

            return View(model);
        }

        [HttpPost, ActionName("ViewChat")]
        [ValidateAntiForgeryToken]
        public ActionResult AddMessage(ChatModel model)
        {
            Chat chat = chatRepository.GetChat(model.ChatId);
            Account account = GetAccount();

            if (ModelState.IsValid) // If a message is entered
            {
                chatLogic.AddMessage(model.MessageInput, account.AccountId, chat);
                return RedirectToAction("ViewChat", "Chat", new { chatId = chat.ChatId }); // Redirect to specific chat
            }
            else ModelState.AddModelError("", "Input is invalid");

            return View(model);
        }

        [HttpGet]
        public ActionResult Remove(int chatId)
        {
            Account account = GetAccount();
            Chat chat = chatRepository.GetChat(chatId);
            RemoveChatModel model = new RemoveChatModel();

            model.Name = chatLogic.GetContactName(chat, account.AccountId);
            model.TotalMessages = chatRepository.GetAllMessages(chat.ChatId).Count();

            return View(model);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int chatId)
        {
            Chat chat = chatRepository.GetChat(chatId);
            chatRepository.RemoveChat(chat);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveMessage(ChatModel model)
        {
            //GroupMessage message = groupRepository.GetGroupMessage(groupMessageId);
            //int rememberedGroupId = message.GroupId; // Keep groupId to reload this chat..            
            //groupRepository.RemoveGroupMessage(message);

            //return RedirectToAction("ViewChat", "Group", new { groupId = rememberedGroupId }); // ..reload chat


            int rememberedMessageId = model.message.MessageId;
            chatRepository.RemoveMessage(model.message);
            return RedirectToAction("ViewChat", "Chat", new { chatId = rememberedMessageId });
        }
    }
}