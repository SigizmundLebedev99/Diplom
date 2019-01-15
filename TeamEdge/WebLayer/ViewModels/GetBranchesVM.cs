using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class GetBranchesVM
    {
        public int? FromUserId { get; set; }
        public string Search { get; set; }
    }
}
