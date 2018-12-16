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
        public Task SendConfirmation(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendInvite(string email, InviteDTO model)
        {
            throw new NotImplementedException();
        }

        public Task SendProjectNotify(User user, ProjectNotifyDTO model)
        {
            throw new NotImplementedException();
        }

        public Task SendWorkItemNotify(User subscriber, IEnumerable<HistoryRecord> messages)
        {
            throw new NotImplementedException();
        }
    }
}
