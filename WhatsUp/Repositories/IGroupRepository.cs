using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsUp.Models.Domain_Models;

namespace WhatsUp.Repositories
{
    interface IGroupRepository
    {
        IEnumerable<Group> GetAllGroups(int accountId);
        void AddGroup(Group group);
        void AddMessage(GroupMessage message);
        void AddGroupAccounts(Group group, Account account);
        Account GetAccount(int accountId);
        Group GetGroup(int groupId);
        void RemoveGroup(Group group);
        GroupMessage GetGroupMessage(int groupMessageId);
        void RemoveGroupMessage(GroupMessage message);
        Contact GetGroupOwnerContact(int groupOwnerId, int accountId);
    }
}
