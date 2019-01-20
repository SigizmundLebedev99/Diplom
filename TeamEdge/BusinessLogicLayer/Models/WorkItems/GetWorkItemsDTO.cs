namespace TeamEdge.Models
{
    public class GetItemsDTO
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }

        public int? ParentId { get; set; }
        public bool HasNoParent { get; set; }
    }
}
