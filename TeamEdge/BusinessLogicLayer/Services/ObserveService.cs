using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class ObserveService : IObserveService
    {
        readonly DbContextOptions<TeamEdgeDbContext> _options;
        readonly IEmailService _emailService;

        public ObserveService(DbContextOptions<TeamEdgeDbContext> options, IEmailService emailService)
        {
            _options = options;
            _emailService = emailService;
        }

        public void WorkItemChanged(int workItemId, WorkItemChanged changes)
        {
            using(var context = new TeamEdgeDbContext(_options))
            {
                var project = context.WorkItemDescriptions.Where(e => e.Id == workItemId).Select(e => new ProjectDTO
                {
                    Id = e.Project.Id,
                    Logo = e.Project.Logo,
                    Name = e.Project.Name
                }).FirstOrDefault();
                var emails = context.Subscribes.Where(e => e.WorkItemId == workItemId).Select(e => e.Subscriber.Email).ToList();
                _emailService.SendItemNotifyAsync(emails, project, changes);
            }
        }
    }
}
