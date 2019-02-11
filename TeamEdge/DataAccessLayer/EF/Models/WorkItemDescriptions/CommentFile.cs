using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Models
{
    public class CommentFile
    {
        public int WorkItemId { get; set; }
        public int FileId { get; set; }
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        public WorkItemFile WorkItemFile { get; set; }
    }
}
