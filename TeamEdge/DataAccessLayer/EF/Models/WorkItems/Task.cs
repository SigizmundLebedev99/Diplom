using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class _Task : BaseWorkItem, IBaseWorkItemWithParent<UserStory>, IBaseWorkItemWithParent<SummaryTask>, IBaseWorkItemWithChild<SubTask>
    {
        public ICollection<SubTask> Children { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [PropertyChanges(typeof(ParentChangeFactory))]
        public UserStory Parent { get; set; }
        public int? AssignedToId { get; set; }
        public TaskType Type { get; set; }

        [PropertyChanges(typeof(AssignedToChangeFactory))]
        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }

        public override string Code => Type.Code();

        public short? TimeSpan { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateFinish { get; set; }

        [Column("SummaryTask")]
        int? IBaseWorkItemWithParent<SummaryTask>.ParentId { get; set; }
        [ForeignKey("SummaryTask")]
        SummaryTask IBaseWorkItemWithParent<SummaryTask>.Parent { get; set; }
    }
}
