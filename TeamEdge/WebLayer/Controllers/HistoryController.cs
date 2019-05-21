using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer;

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
        /// <summary>
        /// Получить историю изменений единицы работы
        /// </summary>
        /// <returns></returns>
        [HttpGet("project/{projectId}/code/{code}/number/{number}")]
        public async Task<IActionResult> GetHistoryForWI(int projectId, string code, int number)
        {
            var res = await _exportService.GetHistoryRecordsForItem(projectId, code, number);
            return Ok(res);
        }

        /// <summary>
        /// Получить список изменений по проекту
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetHistoryForProject(int projectId, [FromQuery]int skip, [FromQuery]int take = 20)
        {
            var res = await _exportService.GetHistoryRecordsForProject(projectId, skip, take);
            return Ok(res);
        }
    }
}
