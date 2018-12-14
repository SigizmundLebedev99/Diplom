using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Models;
using TeamEdge.JWT;
using TeamEdge.Models;

namespace TeamEdge.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly IEmailService _emailService;
        readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Returns jwt token for authorization after authentication
        /// </summary>
        /// <param name="model">email as login and password</param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { Message = "User with current email doesn't exist", Key = "wrongEmail"});
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest(new { Message = "You entered the wrong password", Key = "wrongPass" });
            var token = CreateToken(user);
            return Ok(token);
        }

        /// <summary>
        /// main registration of new users; TODO: Email confirmation;
        /// </summary>
        /// <param name="model">user model</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserDTO model)
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState);
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            result = await _userManager.AddPasswordAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            _emailService.SendConfirmation(user);
            return Ok();
        }

        /// <summary>
        /// Return OAuth providers
        /// </summary>
        /// <returns></returns>
        [HttpGet("providers")]
        public async Task<IActionResult> GetAuthenticationProviders()
        {
            var result = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return Ok(result);
        }

        private TokenResultDTO CreateToken(User user)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            if (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.Firstname))
                claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.Firstname + " " + user.Lastname));
            claims.Add(new Claim("Id", user.Id.ToString()));

            ClaimsIdentity claimsIdentity =
                            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                ClaimsIdentity.DefaultRoleClaimType);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthTokenOptions.ISSUER,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthTokenOptions.LIFETIME)),
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
