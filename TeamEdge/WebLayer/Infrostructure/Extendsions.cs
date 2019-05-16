using Microsoft.AspNetCore.Identity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Models;
using TeamEdge.Models;
using Task = System.Threading.Tasks.Task;

namespace TeamEdge.WebLayer
{
    static public class Extendsions
    {
        public static int Id(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(GetClaim(user, "Id", true));
        }

        public static string Avatar(this ClaimsPrincipal user)
        {
            return GetClaim(user, "Avatar", false);
        }

        public static string Username(this ClaimsPrincipal user)
        {
            return GetClaim(user, "UserName", true);
        }

        public static string Email(this ClaimsPrincipal user)
        {
            return GetClaim(user, "Email", true);
        }

        public static string FullName(this ClaimsPrincipal user)
        {
            return GetClaim(user, "GivenName", true);
        }

        private static string GetClaim(ClaimsPrincipal user, string claim, bool required)
        {
            var Claim = user.Claims.FirstOrDefault(c => c.Type == claim);
            if (required)
                if (Claim == null)
                    throw new UnauthorizedException();
            return Claim?.Value;
        }

        public static UserDTO Model(this ClaimsPrincipal user)
        {
            return new UserDTO
            {
                Id = user.Id(),
                UserName = user.Username(),
                Email = user.Email(),
                Name = user.FullName(),
                Avatar = user.Avatar()
            };
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
