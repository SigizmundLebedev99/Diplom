namespace TeamEdge.Models
{
    public class GetWorkItemsDTO
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
    }
}
