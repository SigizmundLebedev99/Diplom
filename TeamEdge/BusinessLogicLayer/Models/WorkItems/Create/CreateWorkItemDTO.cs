using Newtonsoft.Json;
using TeamEdge.WebLayer;

namespace TeamEdge.Models
{
    [JsonConverter(typeof(WorkItemConverter))]
    public class CreateWorkItemDTO
    {
        public int ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DescriptionText { get; set; }
        public string DescriptionCode { get; set; }
        public int[] FileIds { get; set; }
        public string[] Branches { get; set; }
        public string[] Tags { get; set; }
        public int? ParentId { get; set; }
        public int[] ChildrenIds { get; set; }
        public int CreatorId { get; set; }
        public WorkItemStatus Status { get; set; }
    }
}
