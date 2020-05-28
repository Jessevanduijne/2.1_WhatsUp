using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsUp.Models.Domain_Models;
using WhatsUp.Models.View_Models;
using WhatsUp.Repositories;

namespace WhatsUp.Models.Business_Logic
{
    public class ChatLogic
    {
        IChatRepository chatRepository = new ChatRepository();
        IContactRepository contactRepository = new ContactRepository();
        IAccountRepository accountRepository = new AccountRepository();

        public IEnumerable<ChatMessage> GetLastMessages(int accountId)
        {
            IEnumerable<Chat> chats = chatRepository.GetAllChats(accountId); // Sent & received messages
            List<ChatMessage> lastChatMessages = new List<ChatMessage>();
            
            foreach (Chat c in chats)
            {
                Message latestMessage = c.Messages.OrderByDescending(m => m.MessageSent).FirstOrDefault(); // Get latest message
                ChatMessage message = CreateMessageModel(c, accountId, latestMessage); // If a chat exists with messages
                lastChatMessages.Add(message);
            }
            return lastChatMessages;
        }

        private ChatMessage CreateMessageModel(Chat c, int accountId, Message latestMessage)
        {
            ChatMessage message = new ChatMessage();
            message.Message = latestMessage; // Set domain model in view model           
            bool senderIsReceiver = latestMessage.SenderIsReceiver; // Check who started the chat (if you received first, you're the receiver)
            message.RemoveChatAvailable = false; 

            if (c.SenderId == accountId) // if you're account 1 (sender)..
            {                
                string contactName = contactRepository.GetContact(c.SenderId, c.ReceiverId).ContactName;
                message.OtherAccountContactName = contactName;

                if (senderIsReceiver) message.LastSender = contactName; // account two (other account) sent last message            
                else YouAreTheLastSender(message); // account 1 (you) sent the last message
            }
            else // if you're account 2 (receiver)..
            {                
                Contact contact = contactRepository.GetContact(c.ReceiverId, c.SenderId);
                string unknownContactMobileNumber = accountRepository.GetAccount(c.SenderId).MobileNumber;

                if (contact != null)
                {
                    message.OtherAccountContactName = contact.ContactName; // Show contactName if it is saved by account
                    if (senderIsReceiver) YouAreTheLastSender(message); // account 2 (you) sent the last message
                    else message.LastSender = contact.ContactName;
                }
                else
                {
                    message.OtherAccountContactName = unknownContactMobileNumber; // else show mobileNumber of other account
                    if (senderIsReceiver) YouAreTheLastSender(message); // account 2 (you) sent the last message
                    else message.LastSender = unknownContactMobileNumber; // account 1 (other account) sent the last message
                }                            
            }    
            return message;
        }

        private void YouAreTheLastSender(ChatMessage message)
        {
            message.LastSender = "You";
            message.RemoveChatAvailable = true;
        }

        public string GetContactName(Chat chat, int accountId)
        {
            string contactName = "";

            // This method retrieves the contactName of the opposing chat-partner. If the opposing chat-partner isn't added yet, get it's phonenumber
            if (chat.SenderId == accountId) 
            {
                contactName = contactRepository.GetContact(chat.SenderId, chat.ReceiverId).ContactName;                 
                // As a sender, your contact's mobileNumber can't be unknown
            }
            else 
            {
                Contact contact = contactRepository.GetContact(chat.ReceiverId, chat.SenderId);
                string unknownContactMobileNumber = accountRepository.GetAccount(chat.SenderId).MobileNumber;

                if (contact != null) contactName = contact.ContactName; 
                else contactName = unknownContactMobileNumber;               
            }
            return contactName;       
        }

        public List<ChatMessage> GetAllMessages(Chat chat, int accountId)
        {
            // This method formats all the messages in ChatMessage Model-form
            IEnumerable<Message> messagesInput = chatRepository.GetAllMessages(chat.ChatId);
            List<ChatMessage> messagesOutput = new List<ChatMessage>();
            
            foreach(Message m in messagesInput)
            {
                ChatMessage message = CreateMessageModel(chat, accountId, m);                
                messagesOutput.Add(message);
            }
            return messagesOutput;
        }

        public void AddMessage(string text, int accountId, Chat chat)
        {
            Message message = new Message();
            message.ChatId = chat.ChatId;
            message.MessageSent = DateTime.Now;           
            message.TextMessage = text;

            // Determine whether you're the sender or receiver
            if (chat.ReceiverId == accountId) message.SenderIsReceiver = true;
            else message.SenderIsReceiver = false;

            chatRepository.AddMessage(message);
        }
    }
}