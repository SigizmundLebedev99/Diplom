﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationAsync(UserDTO to, string url);
        void SendWorkItemNotifyAsync(User subscriber, IEnumerable<HistoryRecord> messages);
        Task SendInviteAsync(InviteCodeDTO model);
        void SendProjectNotifyAsync(User user, ProjectNotifyDTO model);
        void NotifyUserLeaveAsync(UserDTO from, int projectId);
        void NotifyUserDeletedAsync(UserDTO from, int userId, int projectId);
        void SendItemNotify(IEnumerable<string> emails, ProjectDTO project, WorkItemChanged changes);
    }
}
