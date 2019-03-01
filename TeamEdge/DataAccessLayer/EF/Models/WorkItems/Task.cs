using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class _Task : BaseWorkItem, IBaseWorkItemWithParent<UserStory>, IBaseWorkItemWithParent<SummaryTask>, IBaseWorkItemWithChild<SubTask>, ITimeConstraint
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

        public short? Duration { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Column("SummaryTask")]
        public int? ParentSummaryTaskId { get; set; }
        [ForeignKey("SummaryTask")]
        public SummaryTask ParentSummaryTask { get; set; }

        private int? prevTaskId { get; set; }
        private int? prevSummaryTaskId { get; set; }
        [ForeignKey("prevTaskId")]
        private _Task prevTask { get; set; }
        [ForeignKey("prevSummaryTaskId")]
        private SummaryTask prevSummaryTask { get; set; }

        int? IBaseWorkItemWithParent<SummaryTask>.ParentId { get => ParentSummaryTaskId; set { ParentSummaryTaskId = value; } }
        SummaryTask IBaseWorkItemWithParent<SummaryTask>.Parent { get => ParentSummaryTask; set => ParentSummaryTask = value; }

        public int? PreviousTaskId { get; set; }
        public int? PreviousSummaryTaskId { get; set; }
        [ForeignKey("PreviousTaskId")]
        public _Task PreviousTask { get; set; }
        [ForeignKey("PreviousSummaryTaskId")]
        private SummaryTask PreviousSummaryTask { get; set; }
    }
}
