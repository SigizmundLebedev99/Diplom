using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.DAL.Mongo;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/history")]
    public class HistoryController : Controller
    {
        readonly ExportHistoryService _exportService;  

        public HistoryController(ExportHistoryService service)
        {
            _exportService = service;
        }

        [HttpGet("project/{projectId}/code/{code}/number/{number}")]
        public async Task<IActionResult> GetHistoryForWI(int projectId, string code, int number, [FromQuery]int skip, [FromQuery]int take = 20)
        {
            var res = await _exportService.GetHistoryRecordsForItem(projectId, code, number, skip, take);
            return Ok(res);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetHistoryForProject(int projectId, [FromQuery]int skip, [FromQuery]int take = 20)
        {
            var res = await _exportService.GetHistoryRecordsForProject(projectId, skip, take);
            return Ok(res);
        }
    }
}
