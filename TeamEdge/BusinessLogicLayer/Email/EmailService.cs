using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Email;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class EmailService : IEmailService
    {
        readonly IHostingEnvironment _env;
        readonly SmtpClient _smtpClient;
        readonly string _from;
        readonly string _fromPass;

        public EmailService(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            _from = config.GetValue<string>("Email:From");
            _fromPass = config.GetValue<string>("Email:Password");
            _smtpClient = new SmtpClient(config.GetValue<string>("Email:Host"), Convert.ToInt32(config.GetValue<string>("Email:Port")))
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(_from, _fromPass)
            };
        }

        public void NotifyUserDeletedAsync(UserDTO from, int userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public void NotifyUserLeaveAsync(UserDTO from, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task SendConfirmationAsync(ConfirmEmailBM model)
        {
            string htmlPath = Path.Combine(_env.ContentRootPath, "AppData/Email/confirm.html");
            var html = MessageBuilder.BuildMessageHtml(htmlPath, model);
            var mailMessage = new MailMessage
            {
                Subject = $"Подтвердите свой email",
                Body = html,
                IsBodyHtml = true,
                From = new MailAddress(_from, "Администрация Team Edge")
            };
            mailMessage.To.Add(model.Email);
            return _smtpClient.SendMailAsync(mailMessage);
        }

        public Task SendInviteAsync(InviteCodeDTO model)
        {
            string htmlPath = Path.Combine(_env.ContentRootPath, "AppData/Email/sendInvite.html");
            var html = MessageBuilder.BuildMessageHtml(htmlPath, model);
            var mailMessage = new MailMessage
            {
                Subject = $"TeamEdge: приглашение в проект \"{model.ProjectName}\"",
                Body = html,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(model.Email);
            return _smtpClient.SendMailAsync(mailMessage);
        }

        public async void SendItemNotify(IEnumerable<string> emails, ProjectDTO project, WorkItemChanged changes)
        {
            string htmlPath = Path.Combine(_env.ContentRootPath, "AppData/Email/workItemChanged.html");
            string html = MessageBuilder.BuildMessageHtml(htmlPath, new { project, changes });
            var mailMessage = new MailMessage
            {
                Subject = $"Уведомление от TeamEdge: {project.Name}",
                Body = html,
                IsBodyHtml = true,
            };
            foreach (var email in emails)
                mailMessage.To.Add(email);
            _smtpClient.Send(mailMessage);
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
