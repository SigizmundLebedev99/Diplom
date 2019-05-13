using TeamEdge.DAL.Models;

namespace TeamEdge.Models
{
    public struct ChangeStatusDTO
    {
        public int FromId { get; set; }

        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public ProjectAccessLevel NewProjLevel { get; set; }
    }
}
