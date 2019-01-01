using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class WorkItemFile : BaseEntity
    {
        public int WorkItemId { get; set; }
        [ForeignKey("WorkItemId")]
        public WorkItemDescription WorkItem { get; set; }

        public string Description { get; set; }
        [StringLength(128, MinimumLength = 3)]
        public string FileName { get; set; }
        [StringLength(512, MinimumLength = 3)]
        public string FilePath { get; set; }
    }
}