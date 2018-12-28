using TeamEdge.DAL.Models;

namespace TeamEdge.Models
{
    public struct TaskDTO
    {
        public int WorkItemId { get; set; }
        public string Name { get; set; }
        public TaskType TaskType { get; set; }
    }
}
