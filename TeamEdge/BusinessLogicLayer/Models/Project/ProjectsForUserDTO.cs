using System.Collections.Generic;

namespace TeamEdge.Models
{
    public class ProjectsForUserDTO
    {
        public IEnumerable<InviteForUserDTO> Invites { get; set; }
        public IEnumerable<ProjectForUserDTO> Projects { get; set; }
    }
}
