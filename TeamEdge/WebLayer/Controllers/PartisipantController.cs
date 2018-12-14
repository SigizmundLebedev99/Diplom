using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    public class PartisipantController : Controller
    {
        public async Task<IActionResult> JoinProject(int inviteId)
        {
            return Ok();
        }

        public async Task<IActionResult> LeaveProject(int projectId)
        {
            return Ok();
        }

        public async Task<IActionResult> ChangePartisipantStatus(ChangeStatusDTO model)
        {
            return Ok();
        }
    }
}
