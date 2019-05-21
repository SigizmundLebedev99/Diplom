using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using TeamEdge.WebLayer;

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
        /// Возвращает JWT для авторизации
        /// </summary>
        [HttpPost("token")]
        [ProducesResponseType(200, Type = typeof(TokenResultDTO))]
        public async Task<IActionResult> Token([FromBody]LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var token = await _accountService.Token(model);
            return Ok(token);
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model"></param>
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
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code },
                protocol: HttpContext.Request.Scheme);

            await _emailService.SendConfirmationAsync(new ConfirmEmailBM
            {
                FullName = user.FullName,
                Url = callbackUrl,
                Email = user.Email,
                Host = Request.Host.Host,
                Port = Request.Host.Port?.ToString(),
                Schema = Request.Scheme
            });
            return Ok();
        }

        /// <summary>
        /// Подтверждение электронного адреса. Возвращает html страницу.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return View(new ConfirmEmailBM { FullName = user.FirstName, Url = Url.Action("Index", "Home")});
            return View("Error");
        }
        /// <summary>
        /// Регистрация по приглашению без подтверждения электронной почты
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("register/code")]
        public async Task<IActionResult> RegisterWithInvite([FromForm]RegisterWithInviteVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _accountService.RegisterWithInvite(model);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("info/{userId}")]
        [ProducesResponseType(200, Type = typeof(UserFullDTO))]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var res = await _accountService.GetUserInfo(userId);
            return Ok(res);
        }

        /// <summary>
        /// Обновление профиля пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody]UpdateUserDTO model)
        {
            var res = await _accountService.UpdateUserInfo(model, User.Id());
            return Ok(res);
        }
    }
}
