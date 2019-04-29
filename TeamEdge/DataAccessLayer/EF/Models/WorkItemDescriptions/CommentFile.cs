using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Models
{
    public class CommentFile
    {
        public int FileId { get; set; }
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        [ForeignKey("FileId")]
        public _File File { get; set; }
    }
}
