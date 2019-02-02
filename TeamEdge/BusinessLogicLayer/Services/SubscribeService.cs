using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class SubscribeService : ISubscribeService
    {
        readonly TeamEdgeDbContext _context;

        public SubscribeService(TeamEdgeDbContext context)
        {
            _context = context;
        }

        public async Task Desubscribe(int userId, int workItemId)
        {
            await Validate(userId, workItemId);
            _context.Subscribes.Remove(new Subscribe { SubscriberId = userId, WorkItemId = workItemId });
            await _context.SaveChangesAsync();
        }

        public async Task Subscribe(int userId, int workItemId)
        {
            await Validate(userId, workItemId);
            _context.Subscribes.Add(new Subscribe { SubscriberId = userId, WorkItemId = workItemId });
            await _context.SaveChangesAsync();
        }

        private async Task Validate(int userId, int workItemId)
        {
            var project = await _context.WorkItemDescriptions.Where(e => e.Id == workItemId).Select(e => e.ProjectId).FirstOrDefaultAsync();
            if (project == 0)
                throw new NotFoundException("item_nf");
            if (!await _context.UserProjects.AnyAsync(e => e.ProjectId == project && e.UserId == userId))
                throw new UnauthorizedException();
            if (await _context.Subscribes.AnyAsync(e => e.SubscriberId == userId && e.WorkItemId == workItemId))
                return;
        }
    }
}
