using TeamEdge.DAL.Models;

namespace TeamEdge.Models
{
    public class ProjectForUserDTO
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public ProjectAccessLevel AccessStatus { get; set; }
        public bool HasTasks { get; set; }
        public int UsersCount { get; set; }
    }
}
