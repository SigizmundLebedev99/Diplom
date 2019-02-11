using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly IEmailService _emailService;
        readonly IMapper _mapper;
        readonly AccountService _accountService;

        public AccountController(UserManager<User> userManager, IMapper mapper, IEmailService emailService, AccountService accountservice)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _accountService = accountservice;
        }

        /// <summary>
        /// Returns jwt token for authorization after authentication
        /// </summary>
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var token = _accountService.Token(model);
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
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code },
                protocol: HttpContext.Request.Scheme);
            await _emailService.SendConfirmationAsync(_mapper.Map<UserDTO>(user), callbackUrl);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("ConfirmationError");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ConfirmationError");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return View("ConfirmEmail", user);
            return View("Error");
        }

        [HttpPost("register/code")]
        public async Task<IActionResult> RegisterWithInvite([FromBody]RegisterWithInviteVM model, string code)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return View("Error");
        }

       
    }
}
