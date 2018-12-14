using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class ProjectsForUserDTO
    {
        public IEnumerable<InviteDTO> Invites { get; set; }
        public IEnumerable<ProjectDTO> Projects { get; set; }
    }
}
