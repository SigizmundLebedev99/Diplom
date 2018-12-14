using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class WorkItemHistory : BaseEntity
    {
        [StringLength(64)]
        public string ObjectId { get; set; }

        public int WorkItemId { get; set; }
        [ForeignKey("WorkItemId")]
        public WorkItemDescription WorkItem { get; set; }
    }
}
