using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface ICommentService
    {
        Task<OperationResult<int>> CreateComment(CreateCommentDTO model);
        Task<IEnumerable<CommentDTO>> GetComments(int userId, int workItemId);
    }
}
