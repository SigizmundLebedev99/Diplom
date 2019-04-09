using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.JWT;
using TeamEdge.Models;

namespace TeamEdge
{
    public class AccountService
    {
        readonly TeamEdgeDbContext _context;
        readonly UserManager<User> _userManager;
        readonly FileSystemService _fileSystemService;
        readonly IMapper _mapper;

        public AccountService(TeamEdgeDbContext context, UserManager<User> userManager, IMapper mapper, FileSystemService fileSystemService)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _fileSystemService = fileSystemService;
        }

        public async Task<TokenResultDTO> RegisterWithInvite(RegisterWithInviteVM model)
        {
            if (model.InviteId == 0)          
                throw new NotFoundException();

            var userId = await _context.Invites.Where(e => e.Id == model.InviteId).Select(e => e.ToUserId).FirstOrDefaultAsync();
            if (userId == 0)
                throw new NotFoundException();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.EmailConfirmed)
            {
                throw new NotFoundException();
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Code);
            if (!result.Succeeded)
                throw new Exception(result.Errors.Select(e => e.Description).Aggregate((s1, s2) => $"{s1}/n{s2}"));
            user.FirstName = model.Firstname;
            user.LastName = model.Lastname;
            user.Patrinymic = model.Patronymic;
            user.UserName = model.UserName;

            await _userManager.UpdateAsync(user);
            await _userManager.AddPasswordAsync(user, model.Password);

            return CreateToken(user);
        }

        public async Task<TokenResultDTO> Token(LoginDTO model)
        {
                var user = await _userManager.FindByEmailAsync(model.Login);
                if (user == null)
                    user = await _userManager.FindByNameAsync(model.Login);
                if (user == null)
                    throw new NotFoundException("user_nf");
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                    throw new NotFoundException("password_inv");
                if (!user.EmailConfirmed)
                    throw new NotFoundException("email_not_confirmed");
                return CreateToken(user);
        }

        public TokenResultDTO CreateToken(User user)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.Email))
                claims.Add(new Claim("Email", user.Email));
            if (!string.IsNullOrEmpty(user.LastName) || !string.IsNullOrEmpty(user.FirstName))
                claims.Add(new Claim("GivenName", user.FirstName + " " + user.LastName));
            if (!string.IsNullOrEmpty(user.UserName))
                claims.Add(new Claim("UserName", user.UserName));
            if (!string.IsNullOrEmpty(user.Avatar))
                claims.Add(new Claim("Avatar", user.UserName));
            claims.Add(new Claim("Id", user.Id.ToString()));

            ClaimsIdentity claimsIdentity =
                            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                ClaimsIdentity.DefaultRoleClaimType);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthTokenOptions.ISSUER,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromHours(AuthTokenOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthTokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenResultDTO
            {
                UserId = user.Id,
                Access_token = token,
                Avatar = user.Avatar,
                Email = user.Email,
                FullName = user.FirstName + " " + user.LastName,
                Start = now,
                Finish = now.Add(TimeSpan.FromMinutes(AuthTokenOptions.LIFETIME))
            };
        }

        public Task<UserFullDTO> GetUserInfo(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(e => new UserFullDTO
            {
                Avatar = e.Avatar,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Patronimic = e.Patrinymic,
                Email = e.Email
            }).FirstOrDefaultAsync();
        }

        public async Task<TokenResultDTO> UpdateUserInfo(UpdateUserDTO model, int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == userId);

            if (user == null)
                throw new NotFoundException("user_nf");
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Patrinymic = model.Patronymic;
            user.Avatar = model.Avatar;
            _fileSystemService.Commit(userId, model.Avatar);
            await _userManager.UpdateAsync(user);
            return CreateToken(user);
        }
    }
}
