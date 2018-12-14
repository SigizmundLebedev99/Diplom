using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace TeamEdge.JWT
{
    public class GoogleTokenValidator : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public GoogleTokenValidator()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            var payload = GoogleJsonWebSignature.ValidateAsync(securityToken, new GoogleJsonWebSignature.ValidationSettings()).Result;

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.GivenName, payload.Name),
                    new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
                    new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
                };

            try
            {
                var principle = new ClaimsPrincipal();
                principle.AddIdentity(new ClaimsIdentity(claims));
                return principle;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
