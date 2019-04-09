namespace TeamEdge.Models
{
    public class ItemWithParentDTO : ItemDTO
    {
        public WorkItemIdentifier Parent { get; set; }
    }

    public class WorkItemIdentifier
    {
        public string Code { get; set; }
        public int Number { get; set; }
    }
}
