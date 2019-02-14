using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    public class RegistrationViewController : Controller
    {
        readonly TeamEdgeDbContext _context;
        readonly UserManager<User> _userManager;

        public RegistrationViewController(TeamEdgeDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> RegisterWithInvite(string code, int inviteId)
        {
            if (string.IsNullOrEmpty(code) || inviteId == 0)
                return View("Error");
            var invite = await _context.Invites.FirstOrDefaultAsync(e => e.Id == inviteId && !e.IsAccepted);
            if (invite == null)
                return View("Error");
            var user = await _userManager.FindByIdAsync(invite.ToUserId.ToString());
            if (user == null)
                return View("Error");
            if (user.EmailConfirmed)
                return View("AlreadyRegistred", new UserExistVM
                {
                    Name = user.FullName, Avatar = user.Avatar, Url = Url.Action("Index","Home")
                });
            return View(new RegisterWithInviteVM { Code = code, InviteId = inviteId});
        }
    }
}
