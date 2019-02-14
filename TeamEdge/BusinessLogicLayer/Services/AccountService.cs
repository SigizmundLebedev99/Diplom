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

        public AccountService(TeamEdgeDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
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
            if (!string.IsNullOrEmpty(user.Lastname) || !string.IsNullOrEmpty(user.Firstname))
                claims.Add(new Claim("GivenName", user.Firstname + " " + user.Lastname));
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
                FullName = user.Firstname + " " + user.Lastname,
                Start = now,
                Finish = now.Add(TimeSpan.FromMinutes(AuthTokenOptions.LIFETIME))
            };
        }
    }
}
