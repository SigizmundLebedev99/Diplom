using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class _Repository : BaseEntity
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
