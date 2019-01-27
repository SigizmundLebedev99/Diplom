﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;

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
                var emails = context.Subscribes.Where(e => e.WorkItemId == workItemId).Select(e => e.Subscriber.Email).ToList();
                _emailService.SendItemNotifyAsync(emails, changes);
            }
        }
    }
}