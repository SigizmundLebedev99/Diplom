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
            var operRes = new OperationResult(true);

            await _validationService.ValidateProject(model.ProjectId, model.UserId, e=>e.IsAdmin);

            _context.UserProjects.Remove(new UserProject { ProjectId = model.ProjectId, UserId = model.UserId });

            await _context.SaveChangesAsync();
        }

        public async Task<OperationResult> LeaveProject(int userId, int projectId)
        {
            var operRes = new OperationResult(true);
            var userProj = await _context.UserProjects.FirstOrDefaultAsync(i => i.UserId == userId && i.ProjectId == projectId);

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

            _context.UserProjects.Remove(userProj);
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
            
            if (invite?.ToUserId != model.UserId || invite.Email != user?.Email || invite.ProjectId == model.ProjectId)
                operRes.AddErrorMessage("user_inv", $"Ошибка доступа");
            if (!operRes.Succeded)
                return operRes;

            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _context.UserProjects.Add(_mapper.Map<UserProject>(invite));
                    invite.IsAccepted = true;
                    _context.Invites.Update(invite);
                    await _context.SaveChangesAsync();
                    transaction.Complete();
                }
                catch(Exception ex)
                {
                    transaction.Dispose();
                    throw;
                }
            }
            return operRes;
        }

        public async Task UpdatePartisipantStatus(ChangeStatusDTO model)
        {
            var operRes = new OperationResult(true);
            await _validationService.ValidateProject(model.ProjectId, model.UserId, e=>e.IsAdmin);
            var userProj = await _context
                .UserProjects
                .AnyAsync(u => u.UserId == model.UserId && u.ProjectId == model.ProjectId);
            if(!userProj)
                throw new NotFoundException("user_nf",
                    $"Не удалось найти пользователя c id = {model.UserId} для проекта c id={model.ProjectId}");

            _context.UserProjects.Update(_mapper.Map<UserProject>(model));
            await _context.SaveChangesAsync();
        }

        public async Task<InviteCodeDTO> CreateInvite(CreateInviteDTO model)
        {
            await _validationService.ValidateProject(model.ProjectId, model.FromUserId, e => e.IsAdmin);
            string code = null;
            var invite = _mapper.Map<Invite>(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
                invite.ToUserId = user.Id;
            else
            {
                string userName = model.Email.Remove(model.Email.IndexOf('@'));
                user = new User { Email = model.Email, UserName = userName };
                await _userManager.CreateAsync(user);
                invite.ToUserId = user.Id;
                code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            invite.DateOfCreation = DateTime.Now;
            _context.Invites.Add(invite);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<InviteCodeDTO>(invite);
            result.Code = code;
            return result;
        }
    }
}
