using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class MembershipService : IMembershipService
    {
        readonly TeamEdgeDbContext _context;
        readonly UserManager<User> _userManager;
        readonly IMapper _mapper;
        readonly IValidationService _validationService;

        public MembershipService(TeamEdgeDbContext context, IMapper mapper, UserManager<User> userManager, IValidationService validationService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _validationService = validationService;
        }

        public async Task DeletePartisipant(DeletePartisipantDTO model)
        {
            var userProject = await _context.UserProjects.FirstOrDefaultAsync(e => e.ProjectId == model.ProjectId && e.UserId == model.UserId);
            if (userProject == null)
                throw new NotFoundException("project_nf");
            userProject.IsDeleted = true;
            _context.UserProjects.Update(userProject);

            await _context.SaveChangesAsync();
        }

        public async Task<OperationResult> LeaveProject(int userId, int projectId)
        {
            var operRes = new OperationResult(true);
            var userProj = await _context.UserProjects.FirstOrDefaultAsync(i => i.UserId == userId && i.ProjectId == projectId && !i.IsDeleted);

            if (userProj == null)
                throw new NotFoundException();

            var adminsCount = await _context
                .UserProjects
                .Where(i => i.ProjectId == projectId && i.ProjRole == ProjectAccessLevel.Administer)
                .Select(i => i.User.Email).CountAsync();

            if (adminsCount == 1 && userProj.ProjRole == ProjectAccessLevel.Administer)
            {
                operRes.AddErrorMessage("leave_single_admin", "Перед тем как покидать проект, назначьте нового администратора");
                return operRes;
            }
            userProj.IsDeleted = true;
            _context.UserProjects.Update(userProj);
            await _context.SaveChangesAsync();
            return operRes;
        }

        public async Task<OperationResult> JoinProject(JoinProjectDTO model)
        {
            var user = await _context
                .Users
                .Select(e => new { e.Id, e.Email, e.EmailConfirmed })
                .FirstOrDefaultAsync(u => u.Id == model.UserId && u.EmailConfirmed);
            if (user == null)
                throw new UnauthorizedException();

            var operRes = new OperationResult(true);
            var invite = await _context.Invites.FirstOrDefaultAsync(i => !i.IsAccepted && i.Id == model.InviteId);
            if (invite == null)
                throw new NotFoundException("invite_nf", $"Не удалось найти инвайт с id = {model.InviteId}");
            
            if (!operRes.Succeded)
                return operRes;
            var newUP = _mapper.Map<UserProject>(invite);
            if (await _context.UserProjects.AnyAsync(e => e.ProjectId == invite.ProjectId && e.UserId == model.UserId))
                _context.UserProjects.Update(newUP);
            else
                _context.UserProjects.Add(newUP);
            invite.IsAccepted = true;
            _context.Invites.Update(invite);
            await _context.SaveChangesAsync();
            
            return operRes;
        }

        public async Task UpdatePartisipantStatus(ChangeStatusDTO model)
        {
            var operRes = new OperationResult(true);
            await _validationService.ValidateProjectAccess(model.ProjectId, model.FromId, e=>e.IsAdmin);
            var entity = _mapper.Map<UserProject>(model);
            entity.ProjRole = model.NewProjLevel;
            _context.UserProjects.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<OperationResult<InviteCodeDTO>> CreateInvite(CreateInviteDTO model)
        {
            var operRes = new OperationResult<InviteCodeDTO>(true);
            await _validationService.ValidateProjectAccess(model.ProjectId, model.FromUserId, e => e.IsAdmin);
            string code = null;
            var invite = _mapper.Map<Invite>(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && user.EmailConfirmed)
            {
                if(await _context.Invites.Select(e=>new { userId = e.ToUserId , e.ProjectId})
                    .Concat(_context.UserProjects.Where(e=>!e.IsDeleted).Select(e=>new { userId = e.UserId, e.ProjectId}))
                    .AnyAsync(e=>e.ProjectId == model.ProjectId && e.userId == user.Id))
                {
                    operRes.AddErrorMessage("user_already_in");
                    return operRes;
                }
                invite.ToUserId = user.Id;
            }
            else if (user != null)
            {
                invite.ToUserId = user.Id;
                code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            else
            {
                string userName = model.Email.Remove(model.Email.IndexOf('@'));
                user = new User { Email = model.Email, UserName = userName };
                var identRes = await _userManager.CreateAsync(user);
                if (!identRes.Succeeded)
                {
                    foreach (var err in identRes.Errors)
                        operRes.AddErrorMessage(err.Code, err.Description);
                    return operRes;
                }
                invite.ToUserId = user.Id;
                code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            invite.DateOfCreation = DateTime.Now;
            _context.Invites.Add(invite);
            await _context.SaveChangesAsync();
            var project = await _context.Projects.Where(e => e.Id == model.ProjectId).Select(e => new { e.Logo, e.Name }).FirstOrDefaultAsync();
            var result = new InviteCodeDTO
            {
                Logo = project.Logo,
                ProjectName = project.Name,
                Code = code,
                DateOfCreation = invite.DateOfCreation,
                Email = user.Email,
                InviteId = invite.Id
            };
            operRes.Result = result;
            return operRes;
        }
    }
}
