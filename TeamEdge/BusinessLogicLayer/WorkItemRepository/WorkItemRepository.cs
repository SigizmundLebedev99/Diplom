using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    abstract public class WorkItemRepository
    {
        protected readonly TeamEdgeDbContext _context;
        protected readonly IMapper _mapper;

        protected async Task<int> GetNumber<T>(int projId) where T : BaseWorkItem
        {
            if (await _context.Set<T>().Where(e => e.Description.ProjectId == projId).AnyAsync())
            {
                var number = await _context.Set<T>()
                    .Where(e => e.Description.ProjectId == projId)
                    .Select(e => e.Number)
                    .MaxAsync();
                return number + 1;
            }
            else
                return 1;
        }

        public WorkItemRepository(TeamEdgeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public abstract Task<WorkItemDTO> GetWorkItem(int number, int project);
        public abstract Task<OperationResult<WorkItemDTO>> CreateWorkItem(int descriptionId, CreateWorkItemDTO model);
    }
}
