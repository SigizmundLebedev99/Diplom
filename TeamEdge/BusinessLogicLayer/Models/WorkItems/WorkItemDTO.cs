using System.Collections.Generic;

namespace TeamEdge.Models
{
    public class WorkItemDTO
    {
        public string Code { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int DescriptionId { get; set; }
        public WorkItemStatus Status { get; set; }
        public ItemDTO Parent { get; set; }
        public IEnumerable<ItemDTO> Children { get; set; }
        public DescriptionDTO Description { get; set; }
    }
}
