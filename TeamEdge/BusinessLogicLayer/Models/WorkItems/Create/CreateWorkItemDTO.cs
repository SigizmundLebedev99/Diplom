namespace TeamEdge.Models
{
    public class CreateWorkItemDTO
    {
        public int ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DescriptionText { get; set; }
        public string DescriptionCode { get; set; }
        public FileDTO[] Files { get; set; }
        public string[] Branches { get; set; }
        public int? ParentId { get; set; }
        public int[] ChildrenIds { get; set; }
        public int CreatorId { get; set; }
    }
}
