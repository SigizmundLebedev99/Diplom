using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public struct InviteForUserDTO
    {
        public int InviteId { get; set; }
        public int FromId { get; set; }
        public string FromFullName { get; set; }
        public string FromAvatar { get; set; }
        public string FromEmail { get; set; }
        public string ProjectName { get; set; }
        public string Logo { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
