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
            var userProject = await _context.UserProjects.FirstOrDefaultAsync(e => e.ProjectId == model.ProjectId && model.UserId == model.UserId);
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
            
            if (invite.ToUserId != model.UserId || invite.ProjectId == model.ProjectId)
                operRes.AddErrorMessage("user_inv", $"Ошибка доступа");
            if (!operRes.Succeded)
                return operRes;

            _context.UserProjects.Add(_mapper.Map<UserProject>(invite));
            invite.IsAccepted = true;
            _context.Invites.Update(invite);
            await _context.SaveChangesAsync();
            
            return operRes;
        }

        public async Task UpdatePartisipantStatus(ChangeStatusDTO model)
        {
            var operRes = new OperationResult(true);
            await _validationService.ValidateProjectAccess(model.ProjectId, model.UserId, e=>e.IsAdmin);
            //var userProj = await _context
            //    .UserProjects
            //    .AnyAsync(u => u.UserId == model.UserId && u.ProjectId == model.ProjectId && !u.IsDeleted);
            //if(!userProj)
            //    throw new NotFoundException("user_nf",
            //        $"Не удалось найти пользователя c id = {model.UserId} для проекта c id={model.ProjectId}");

            _context.UserProjects.Update(_mapper.Map<UserProject>(model));
            await _context.SaveChangesAsync();
        }

        public async Task<OperationResult<InviteCodeDTO>> CreateInvite(CreateInviteDTO model)
        {
            var operRes = new OperationResult<InviteCodeDTO>(true);
            await _validationService.ValidateProjectAccess(model.ProjectId, model.FromUserId, e => e.IsAdmin);
            string code = null;
            var invite = _mapper.Map<Invite>(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if(await _context.Invites.Select(e=>new { userId = e.ToUserId , e.ProjectId})
                    .Concat(_context.UserProjects.Select(e=>new { userId = e.UserId, e.ProjectId}))
                    .AnyAsync(e=>e.ProjectId == model.ProjectId && e.userId == user.Id))
                {
                    operRes.AddErrorMessage("user_already_in");
                    return operRes;
                }
                invite.ToUserId = user.Id;
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
