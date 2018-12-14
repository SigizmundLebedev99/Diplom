using System.Collections.Generic;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    interface IEmailService
    {
        Task SendConfirmation(User user);
        Task SendWorkItemNotify(User subscriber, IEnumerable<HistoryRecord> messages);
        Task SendInvite(string email, InviteDTO model);
        Task SendProjectNotify(User user, ProjectNotifyDTO model);
    }
}
