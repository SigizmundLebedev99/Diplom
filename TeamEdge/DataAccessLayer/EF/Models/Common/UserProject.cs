using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class UserProject
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public bool IsDeleted { get; set; }

        public ProjectAccessLevel ProjRole { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public bool IsAdmin
        {
            get
            {
                return ProjRole == ProjectAccessLevel.Administer;
            }
        }
    }

    public enum ProjectAccessLevel
    {
        Write,
        Administer
    }
}
