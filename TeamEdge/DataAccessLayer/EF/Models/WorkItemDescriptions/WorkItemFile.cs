using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class WorkItemFile
    {
        public int FileId { get; set; }
        public int WorkItemId { get; set; }

        [ForeignKey("FileId")]
        public File File { get; set; }
        [ForeignKey("WorkItemId")]
        public WorkItemDescription WorkItem { get; set; }
    }
}
