using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class GantService
    {
        readonly TeamEdgeDbContext _context;
        public GantService(TeamEdgeDbContext context)
        {
            _context = context;
        }

        public async Task<GantDiagramDTO> GetGantDiagram(int projectId)
        {
            var items = await _context.SummaryTasks.Where(e => e.Description.ProjectId == projectId).Select(e =>
              new SummaryChainDTO
              {
                  Code = e.Code,
                  DescriptionId = e.DescriptionId,
                  Name = e.Name,
                  Number = e.Number,
                  Status = e.Status,
                  ParentId = e.ParentSummaryTaskId,
                  Children = new List<GantChainDTO>()
              }).Concat(_context.Tasks.Select(e=>
              new GantChainDTO
              {
                  Code = e.Code,
                  DescriptionId = e.DescriptionId,
                  Name = e.Name,
                  Number = e.Number,
                  Status = e.Status,
                  ParentId = e.ParentSummaryTaskId
              })).ToDictionaryAsync(e=>e.DescriptionId);

            foreach(var el in items)
            {
                if(el.Value.ParentId != null)
                {
                    (items[el.Value.ParentId.Value] as SummaryChainDTO).Children.Add(el.Value);
                }

                if(el.Value.PreviousId != null)
                {
                    items[el.Value.ParentId.Value].Next = el.Value;
                }
            }

            return new GantDiagramDTO
            {
                Elements = items.Select(e => e.Value).Where(e => e.ParentId == null)
            };
        }
    }
}
