using TeamEdge.DAL.Models;

namespace TeamEdge.Models
{
    public struct ProjectNotifyDTO
    {
        public int FromId { get; set; }
        public string FromFullName { get; set; }
        public string FromEmail { get; set; }
        public ProjectAccessLevel? NewRole { get; set; }
        public bool Deleted { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
