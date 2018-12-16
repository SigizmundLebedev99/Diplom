using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public struct JoinProjectDTO
    {
        public int InviteId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public int ProjectId { get; set; }
    }
}
