using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class WorkItemDescription : BaseEntity
    {
        [StringLength(8)]
        [Required]
        public string Code;
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public string DescriptionText { get; set; }
        public string DescriptionCode { get; set; }

        public int? LastUpdaterId { get; set; }
        [ForeignKey("LastUpdaterId")]
        public User LastUpdater { get; set; }
        public DateTime? LastUpdate { get; set; }

        public ICollection<BranchLink> Branches { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<WorkItemFile> Files { get; set; }
        public ICollection<WorkItemHistory> History { get; set; }
        public ICollection<Subscribe> Subscribers { get; set; }
    }
}
