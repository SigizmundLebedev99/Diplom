using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class EmailService : IEmailService
    {
        public void NotifyUserDeletedAsync(UserDTO from, int userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public void NotifyUserLeaveAsync(UserDTO from, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task SendConfirmationAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendInviteAsync(UserDTO from, InviteDTO model)
        {
            throw new NotImplementedException();
        }

        public Task SendItemNotifyAsync(IEnumerable<string> emails, ProjectDTO project, WorkItemChanged changes)
        {
            throw new NotImplementedException();
        }

        public void SendProjectNotifyAsync(User user, ProjectNotifyDTO model)
        {
            throw new NotImplementedException();
        }

        public void SendWorkItemNotifyAsync(User subscriber, IEnumerable<HistoryRecord> messages)
        {
            throw new NotImplementedException();
        }
    }
}
