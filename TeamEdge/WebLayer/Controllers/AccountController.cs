using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
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

        public AccountController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        /// <summary>
        /// Returns jwt token for authorization after authentication
        /// </summary>
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Login);
            if (user == null)
                user = await _userManager.FindByNameAsync(model.Login);
            if (user == null)
                return BadRequest(new ErrorMessage{ Message = "User with current email doesn't exist", Alias = "wrongEmail"});
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest(new ErrorMessage{ Message = "You entered the wrong password", Alias = "wrongPass" });
            var token = CreateToken(user);
            return Ok(token);
        }

        /// <summary>
        /// main registration of new users; TODO: Email confirmation;
        /// </summary>
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
            await _emailService.SendConfirmationAsync(user);
            return Ok();
        }

        /// <summary>
        /// Return OAuth providers
        /// </summary>
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
            if (!string.IsNullOrEmpty(user.Lastname) || !string.IsNullOrEmpty(user.Firstname))
                claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.Firstname + " " + user.Lastname));
            if (!string.IsNullOrEmpty(user.UserName))
                claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
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
