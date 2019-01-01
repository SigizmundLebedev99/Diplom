using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class Invite : BaseEntity
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public string Email { get; set; }

        public int? ToUserId { get; set; }
        public User User { get; set; }

        public RepositoryAccessLevel RepoRole { get; set; }
        public ProjectAccessLevel ProjRole { get; set; }

        public bool IsAccepted { get; set; }
    }
}
