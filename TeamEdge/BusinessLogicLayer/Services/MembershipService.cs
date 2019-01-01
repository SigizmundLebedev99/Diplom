using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TeamEdge.BusinessLogicLayer.Infrastructure;
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

        public MembershipService(TeamEdgeDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task DeletePartisipant(DeletePartisipantDTO model)
        {
            var operRes = new OperationResult(true);

            var fromUserPrj = await _context
                .UserProjects
                .AnyAsync(e => e.ProjectId == model.ProjectId 
                && e.UserId == model.FromId 
                && e.ProjRole == ProjectAccessLevel.Administer);

            var toUserPrj = await _context
                .UserProjects
                .Include(e=>e.User.Email)
                .FirstOrDefaultAsync(e => e.ProjectId == model.ProjectId && e.UserId == model.UserId);

            if (!fromUserPrj)
                throw new UnauthorizedException();
            if (toUserPrj == null)
                throw new NotFoundException();

            _context.UserProjects.Remove(toUserPrj);

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
            var fromUserProj = await _context
                .UserProjects
                .AnyAsync(u => u.UserId == model.FromId && u.ProjectId == model.ProjectId && u.IsAdmin);
            if (!fromUserProj)
                throw new UnauthorizedException();
            var userProj = await _context
                .UserProjects
                .AnyAsync(u => u.UserId == model.UserId && u.ProjectId == model.ProjectId);
            if(!userProj)
                throw new NotFoundException("user_nf",
                    $"Не удалось найти пользователя c id = {model.UserId} для проекта c id={model.ProjectId}");

            _context.UserProjects.Update(_mapper.Map<UserProject>(model));
            await _context.SaveChangesAsync();
        }

        public async Task<InviteDTO> CreateInvite(CreateInviteDTO model)
        {
            if (!await _context.Projects.AnyAsync(e => e.Id == model.ProjectId))
                throw new NotFoundException();
            bool isFromAdmin = await _context
                .UserProjects
                .AnyAsync(u => u.ProjectId == model.ProjectId && u.UserId == model.FromUserId && u.IsAdmin);
            if (!isFromAdmin)
                throw new UnauthorizedException();

            var invite = _mapper.Map<Invite>(model);
            var user = _userManager.FindByEmailAsync(model.Email);
            if (user != null)
                invite.ToUserId = user.Id;
            invite.DateOfCreation = DateTime.Now;
            var result = _context.Invites.Add(invite).Entity;
            await _context.SaveChangesAsync();
            return _mapper.Map<InviteDTO>(result);
        }
    }
}
