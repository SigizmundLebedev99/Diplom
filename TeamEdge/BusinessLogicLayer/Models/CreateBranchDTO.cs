using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class CreateBranchDTO
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Branch { get; set; }
        public string FromSha { get; set; }
    }
}
