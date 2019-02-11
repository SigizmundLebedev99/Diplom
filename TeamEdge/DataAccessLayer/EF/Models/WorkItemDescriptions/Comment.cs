using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Text { get; set; }
        public int WorkItemId { get; set; }
        [ForeignKey("WorkItemId")]
        public WorkItemDescription WorkItem { get; set; }

        public virtual ICollection<CommentFile> Files { get; set; }
    }
}
