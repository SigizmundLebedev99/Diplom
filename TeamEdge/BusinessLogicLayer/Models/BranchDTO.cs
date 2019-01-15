using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class BranchDTO
    {
        public string Name { get; set; }
        public UserLightDTO Creator { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
