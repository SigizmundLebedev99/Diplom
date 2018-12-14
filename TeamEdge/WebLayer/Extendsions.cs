using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;
using Task = System.Threading.Tasks.Task;

namespace TeamEdge.WebLayer
{
    public static class Extendsions
    {
        public static int Id(this ClaimsPrincipal user)
        {
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == "Id");
            if (idClaim != null)
                return Convert.ToInt32(idClaim.Value);
            else
                throw new UnauthorizedAccessException();
        }

        public static string Name(this ClaimsPrincipal user)
        {
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);
            if (idClaim != null)
                return Convert.ToInt32(idClaim.Value);
            else
                throw new UnauthorizedAccessException();
        }

        public static Task<User> GetUser(this UserManager<User> userManager, ClaimsPrincipal user)
        {
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == "Id");
            if (idClaim != null)
                return userManager.FindByIdAsync(idClaim.Value);

            var emailClaim = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            if (emailClaim != null)
                return userManager.FindByEmailAsync(emailClaim.Value);

            return Task.FromResult<User>(null);
        }
    }
}
