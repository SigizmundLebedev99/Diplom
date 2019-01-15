namespace TeamEdge.Models
{
    public class CreateTaskDTO : CreateWorkItemDTO
    {
        public int? AssignedToId { get; set; }
    }
}
