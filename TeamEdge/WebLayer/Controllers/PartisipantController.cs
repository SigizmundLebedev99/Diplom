using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    [Route("api/partisipant")]
    public class PartisipantController : Controller
    {
        readonly IMembershipService _membershipService;
        IEmailService _emailService;
        readonly IMapper _mapper;

        public PartisipantController(IMembershipService membershipService, IMapper mapper, IEmailService emailService)
        {
            _membershipService = membershipService;
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// Create invite for new project partisipant
        /// </summary>
        [HttpPost("invite")]
        [ProducesResponseType(200, Type = typeof(InviteDTO))]
        public async Task<IActionResult> SendInvite([FromBody]CreateInviteVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dto = _mapper.Map<CreateInviteDTO>(model);
            dto.FromUserId = User.Id();
            var result = await _membershipService.CreateInvite(dto);
            await _emailService.SendInviteAsync(User.Model(), result);
            return Ok(result);
        }

        /// <summary>
        /// Accept invite and join to project
        /// </summary>
        [HttpPost("join/{inviteId}")]
        public async Task<IActionResult> JoinProject(int inviteId)
        {
            var operRes = await _membershipService.JoinProject(new JoinProjectDTO {
                InviteId = inviteId,
                UserId = User.Id(),
                Email = User.Email()});
            return operRes.GetResult();
        }
        
        /// <summary>
        /// Remove participant from project
        /// </summary>
        [HttpDelete("project/{projectId}/user/{userId}")]
        public async Task<IActionResult> LeaveProject(int projectId, int userId)
        {
            var selfLeaving = User.Id() == userId;

            if (selfLeaving)
            {
                var result = await _membershipService.LeaveProject(userId, projectId);
                if (result.Succeded)
                    _emailService.NotifyUserLeaveAsync(User.Model(), projectId);
                return result.GetResult();
            }
            else
            {
                await _membershipService.DeletePartisipant(new DeletePartisipantDTO()
                {
                    FromId = User.Id(),
                    ProjectId = projectId,
                    UserId = userId
                });
                _emailService.NotifyUserDeletedAsync(User.Model(), userId, projectId);
                return Ok();
            }
        }


        /// <summary>
        /// Change partisipant status
        /// </summary>
        [HttpPut("status")]
        public async Task<IActionResult> ChangePartisipantStatus(ChangeStatusVM model)
        {
            var dto = _mapper.Map<ChangeStatusDTO>(model);
            dto.UserId = User.Id();
            await _membershipService.UpdatePartisipantStatus(dto);
            return Ok();
        }
    }
}
