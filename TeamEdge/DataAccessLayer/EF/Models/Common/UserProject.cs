using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class UserProject
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        public RepositoryAccessLevel Role { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }

    public enum RepositoryAccessLevel
    {
        /// <summary>
        /// User can read or clone a repository
        /// </summary>
        Pull,
        /// <summary>
        /// User can push to a repository
        /// </summary>
        Push,
        /// <summary>
        /// User can change repository settings
        /// </summary>
        Administer
    }
}
