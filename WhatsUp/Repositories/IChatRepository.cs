using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Repositories
{
    interface IChatRepository
    {
        IEnumerable<Chat> GetAllChats(int accountId);
        void AddChat(Chat chat);
        void RemoveChat(Chat chat);
        Chat GetChat(int chatId);
        IEnumerable<Message> GetAllMessages(int chatId);
        void AddMessage(Message message);
        Boolean CheckForDuplicateChats(int receiverId, int senderId);
        void RemoveMessage(Message message);
    }
}
