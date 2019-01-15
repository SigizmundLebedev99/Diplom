using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class GetBranchesDTO
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int? FromUserId { get; set; }
        public string Search { get; set; }
    }
}
