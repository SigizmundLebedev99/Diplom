using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;

namespace TeamEdge.Models
{
    public struct ChangeStatusVM
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public ProjectAccessLevel NewProjLevel { get; set; }
    }
}
