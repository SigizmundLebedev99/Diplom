namespace TeamEdge.Models
{
    public class CreateTaskDTO : CreateWorkItemDTO
    {
        public TaskType Type { get; set; }
        public int? AssignedToId { get; set; }
    }
}
