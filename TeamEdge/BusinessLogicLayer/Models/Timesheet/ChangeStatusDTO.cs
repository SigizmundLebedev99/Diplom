namespace TeamEdge.Models
{
    public class ChangeWorkItemStatusDTO
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int workItemId { get; set; }
        public WorkItemStatus Status { get; set; }
    }
}
