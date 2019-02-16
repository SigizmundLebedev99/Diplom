using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.BusinessLogicLayer.Xls;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class XlsGenerationService
    {
        readonly TeamEdgeDbContext _context;
        readonly IValidationService _validationService;
        readonly IMapper _mapper;

        public XlsGenerationService(TeamEdgeDbContext context, IValidationService service, IMapper mapper)
        {
            _validationService = service;
            _context = context;
            _mapper = mapper;
        }

        internal async Task<byte[]> TasksForParticipant(int userId, int projectId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);

            var tasksForUser = await _context.Tasks.Where(e => e.AssignedToId == userId)
                .Select(e => new TaskXlsDTO
                {
                    Name = e.Name,
                    Number = $"{e.Code}-{e.Number}",
                    //DateStart = e.DateStart,
                    //DateFinish = e.DateFinish,
                }).ToListAsync();

            var tasks = _mapper.Map<List<TaskXlsDTO>>(tasksForUser);

            return XlsBuilder.GenereateXls(tasks);
        }
    }
}
