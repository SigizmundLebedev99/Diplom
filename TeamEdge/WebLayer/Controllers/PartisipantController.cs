using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    public class PartisipantController : Controller
    {
        readonly IMembershipService _membershipService;
        readonly TeamEdgeDbContext _context;
        readonly IMapper _mapper;

        public PartisipantController(IMembershipService membershipService, IMapper mapper)
        {
            _membershipService = membershipService;
            _mapper = mapper;
        }

        public async Task<IActionResult> SendInvite(CreateInviteVM model)
        {
            var dto = _mapper.Map<CreateInviteDTO>(model);
            dto.FromUserId = User.Id();
            var operRes = await _membershipService.SendInvite(dto);
            return operRes.GetResult();
        }

        public async Task<IActionResult> JoinProject(int inviteId)
        {
            var operRes = await _membershipService.JoinProject(new JoinProjectDTO {
                InviteId = inviteId,
                UserId = User.Id(),
                Email = User.Email()});
            return operRes.GetResult();
        }

        public async Task<IActionResult> LeaveProject(int projectId, int userId)
        {
            var result = await _membershipService.DeletePartisipant(new DeletePartisipantDTO()
            {
                FromId = User.Id(),
                ProjectId = projectId,
                UserId = userId
            });

            return result.GetResult();
        }

        public async Task<IActionResult> ChangePartisipantStatus(ChangeStatusVM model)
        {
            var dto = _mapper.Map<ChangeStatusDTO>(model);
            dto.UserId = User.Id();

            var result = await _membershipService.UpdatePartisipantStatus(dto);

            return result.GetResult();
        }
    }
}
