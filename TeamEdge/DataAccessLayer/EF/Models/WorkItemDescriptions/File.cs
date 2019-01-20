using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class _File : BaseEntity
    {
        public virtual ICollection<WorkItemFile> WorkItemFiles { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [StringLength(128, MinimumLength = 3)]
        public string FileName { get; set; }
        [StringLength(512, MinimumLength = 3)]
        public string FilePath { get; set; }
    }
}