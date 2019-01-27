using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class WorkItemDescription : BaseEntity
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [PropertyChanges(typeof(SimpleChangeFactory))]
        public string DescriptionText { get; set; }

        public int? LastUpdaterId { get; set; }
        [ForeignKey("LastUpdaterId")]
        public User LastUpdater { get; set; }
        public DateTime? LastUpdate { get; set; }

        public ICollection<BranchLink> Branches { get; set; }
        public ICollection<Comment> Comments { get; set; }
        [PropertyChanges(typeof(FileChangeFactory))]
        public ICollection<WorkItemFile> Files { get; set; }
        public ICollection<Subscribe> Subscribers { get; set; }
    }
}
