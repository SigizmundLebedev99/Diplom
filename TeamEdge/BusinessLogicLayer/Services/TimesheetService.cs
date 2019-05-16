using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;
using Microsoft.EntityFrameworkCore;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using System;
using System.Collections.Generic;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class TimesheetService
    {
        readonly TeamEdgeDbContext _context;
        readonly IValidationService _validationService;

        public TimesheetService(TeamEdgeDbContext context, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task ChangeStatus(ChangeWorkItemStatusDTO model)
        {
            await _validationService.ValidateProjectAccess(model.ProjectId, model.UserId);
            BaseWorkItem item =
                await ((IQueryable<BaseWorkItem>)_context.Tasks.Where(e => e.Description.ProjectId == model.ProjectId && e.DescriptionId == model.workItemId))
                .Concat(_context.SubTasks.Include(e=>e.Parent).Where(e => e.Description.ProjectId == model.ProjectId && e.DescriptionId == model.workItemId))
                .FirstOrDefaultAsync();
            if (item == null)
                throw new NotFoundException("item_nf");
            if (item is _Task)
                await ChangeTaskStatus(item as _Task, model);
            else
                await ChangeSubTaskStatus(item as SubTask, model);
        }

        public async Task<IEnumerable<TimesheetDTO>> GetTimesheets(int id)
        {
            var timesheets = await _context.Timesheets.Where(e => e.TaskId == id || e.SubTaskId == id)
                .Select(e=>new TimesheetDTO
                {
                    ChangedBy = new UserLightDTO
                    {
                        Avatar = e.Creator.Avatar,
                        Id = e.CreatorId,
                        Name = e.Creator.FullName
                    },
                    StartDate =e.StartDate,
                    EndDate = e.EndDate,
                    EndsWith = e.EndsWith,
                    EndedBy = e.EndedBy == null?null: new UserLightDTO
                    {
                        Avatar = e.EndedBy.Avatar,
                        Id = e.EndedById.Value,
                        Name = e.EndedBy.FullName
                    },
                    Subtask = e.SubTask == null?null:new ItemDTO
                    {
                        Code = "SUBTASK",
                        Status = e.SubTask.Status,
                        Name = e.SubTask.Name,
                        Number = e.SubTask.Number
                    }
                })
                .ToListAsync();
            return timesheets;
        }

        private async Task ChangeTaskStatus(_Task task, ChangeWorkItemStatusDTO model)
        {
            var status = model.Status;
            if (task.Status == WorkItemStatus.New)
                if (status != WorkItemStatus.Active)
                    return;
            if(status == WorkItemStatus.Closed || status == WorkItemStatus.Stoped)
            {
                var sheet = await _context.Timesheets.FirstOrDefaultAsync(e => e.TaskId == task.DescriptionId && e.EndDate == null);
                sheet.EndDate = DateTime.Now;
                sheet.EndsWith = status;
                sheet.EndedById = model.UserId;
                _context.Timesheets.Update(sheet);
            }
            if(status == WorkItemStatus.Active)
            {
                var sheet = new Timesheet
                {
                    StartDate = DateTime.Now,
                    CreatorId = model.UserId,
                    TaskId = task.DescriptionId,
                    DateOfCreation = DateTime.Now
                };
                _context.Timesheets.Add(sheet);
            }
            task.Status = status;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        private async Task ChangeSubTaskStatus(SubTask subtask, ChangeWorkItemStatusDTO model)
        {
            var status = model.Status;
            if (subtask.Status == WorkItemStatus.New)
                if (status != WorkItemStatus.Active)
                    return;
            if (status == WorkItemStatus.Closed || status == WorkItemStatus.Stoped)
            {
                var sheet = await _context.Timesheets.FirstOrDefaultAsync(e => e.SubTaskId == subtask.DescriptionId && e.EndDate == null);
                sheet.EndDate = DateTime.Now;
                sheet.EndsWith = status;
                sheet.EndedById = model.UserId;
                _context.Timesheets.Update(sheet);
            }
            if (status == WorkItemStatus.Active)
            {
                var sheet = new Timesheet
                {
                    StartDate = DateTime.Now,
                    CreatorId = model.UserId,
                    TaskId = subtask.ParentId.Value,
                    SubTaskId = subtask.DescriptionId,
                    DateOfCreation = DateTime.Now
                };
                _context.Timesheets.Add(sheet);
            }
            subtask.Status = status;
            _context.SubTasks.Update(subtask);
            await _context.SaveChangesAsync();
        }
    }
}
