using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class WorkItemTag
    {
        [StringLength(32, MinimumLength =1)]
        public string Tag { get; set; }

        public int WorkItemDescId { get; set; }
        [ForeignKey("WorkItemDescId")]
        public WorkItemDescription WorkItem { get; set; }
    }
}
