using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class WorkItemDescription
    {
        public int Id { get; set; }
        public WorkItemType WorkItemType { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public string DescriptionText { get; set; }
        public string DescriptionCode { get; set; }

        public ICollection<BranchLink> CodeLinks { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<WorkItemFile> Files { get; set; }
        public ICollection<WorkItemHistory> History { get; set; }
        public ICollection<Subscribe> Subscribers { get; set; }
    }
}
