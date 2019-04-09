using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class _Task : BaseWorkItem, IBaseWorkItemWithParent<UserStory>, IBaseWorkItemWithChild<SubTask>, ITimeConstraint
    {
        public ICollection<SubTask> Children { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [PropertyChanges(typeof(ParentChangeFactory))]
        public UserStory Parent { get; set; }
        public int? AssignedToId { get; set; }
        public TaskType Type { get; set; }

        public int? EpickId { get; set; }
        public Epick Epick { get; set; }

        [PropertyChanges(typeof(AssignedToChangeFactory))]
        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }

        public override string Code => Type.Code();

        public short? Duration { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? ParentSummaryTaskId { get; set; }
        [ForeignKey("ParentSummaryTaskId")]
        public SummaryTask ParentSummaryTask { get; set; }

        public int? GantPreviousId { get; set; }
        [ForeignKey("GantPreviousId")]
        public WorkItemDescription GantPrevious { get; set; }
    }
}
