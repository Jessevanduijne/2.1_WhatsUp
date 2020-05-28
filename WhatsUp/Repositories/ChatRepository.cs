using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsUp.Models.Database_Connection;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Repositories
{
    public class ChatRepository : IChatRepository
    {
        WhatsUpContext db = new WhatsUpContext();

        public IEnumerable<Chat> GetAllChats(int accountId)
        {
            List<Chat> chats = db.Chats.Where(m => m.ReceiverId == accountId || m.SenderId == accountId).ToList();
            return chats;
        }

        public Chat GetChat(int chatId)
        {
            Chat chat = db.Chats.Find(chatId);
            return chat;
        }

        public void AddChat(Chat chat)
        {
            db.Chats.Add(chat);
            db.SaveChanges();
        }

        public void RemoveChat(Chat chat)
        {
            db.Chats.Remove(chat);
            db.SaveChanges();
        }

        public IEnumerable<Message> GetAllMessages(int chatId)
        {
            IEnumerable<Message> messages = db.Messages.Where(x => x.ChatId == chatId).ToList();
            return messages;
        }

        public void AddMessage(Message message)
        {
            db.Messages.Add(message);
            db.SaveChanges();
        }

        public Boolean CheckForDuplicateChats(int receiverId, int senderId)
        {
            bool chatCheck = db.Chats.Any(x => (x.ReceiverId == receiverId && x.SenderId == senderId) ||
                                                      (x.ReceiverId == senderId && x.SenderId == receiverId));            
            return chatCheck;
        }

        public void RemoveMessage(Message message)
        {
            db.Messages.Remove(message);
            db.SaveChanges();
        }
    }
}