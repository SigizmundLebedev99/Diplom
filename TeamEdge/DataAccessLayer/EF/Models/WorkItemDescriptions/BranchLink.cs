using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class BranchLink : BaseEntity
    {
        public string Branch { get; set; }
        public int WorkItemId { get; set; }
        [ForeignKey("WorkItemId")]
        public WorkItemDescription WorkItem { get; set; }
    }
}
