using TeamEdge.DAL.Models;

namespace TeamEdge.Models
{
    public struct ChangeStatusDTO
    {
        public int UserId { get; set; }
        public UserProjectRole NewRole { get; set; }
    }
}
