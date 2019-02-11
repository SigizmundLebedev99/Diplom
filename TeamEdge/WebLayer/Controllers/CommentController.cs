using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        readonly IMapper _mapper;
        readonly ICommentService _commentService;

        public CommentController(IMapper mapper, ICommentService commentService)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new comment for workItem
        /// </summary>
        /// <returns>id of created entity</returns>
        public async Task<IActionResult> CreateComment([FromBody]CreateCommentVM model)
        {
            var dto = _mapper.Map<CreateCommentDTO>(model);
            dto.From = User.Model();
            var res = await _commentService.CreateComment(dto);
            return res.GetResult();
        }

        /// <summary>
        ///  Get list of comments for workItem
        /// </summary>
        public async Task<IActionResult> GetComments(int workItemId, int skip = 0, int take = 20)
        {
            var res = await _commentService.GetComments(User.Id(), workItemId, skip, take);
            return Ok(res);
        }
    }
}
