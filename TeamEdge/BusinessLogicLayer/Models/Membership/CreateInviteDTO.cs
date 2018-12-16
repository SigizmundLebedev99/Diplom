using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public struct CreateInviteDTO
    {
        public int FromUserId { get; set; }
        public string Email { get; set; }
        public int ProjectId { get; set; }
    }
}
