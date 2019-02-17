using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Mongo;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/history")]
    [Authorize]
    public class HistoryController : Controller
    {
        readonly IMongoContext _context;

        public HistoryController(IMongoContext context)
        {
            _context = context;
        }

        [HttpGet("item/code/{code}/number/{number}")]
        public async Task<IActionResult> GetHistoryForWI(string code, int number, [FromQuery]int skip, [FromQuery]int take = 20)
        {
            return Ok();
        }
    }
}
