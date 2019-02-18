using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Helpers;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class _Task : BaseWorkItem, IBaseWorkItemWithParent<UserStory>, IBaseWorkItemWithParent<SummaryTask>, IBaseWorkItemWithChild<SubTask>, IGauntItem
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

        private int? prevTaskId { get; set; }
        private int? prevSummaryTaskId { get; set; }
        [ForeignKey("prevTaskId")]
        private _Task prevTask { get; set; }
        [ForeignKey("prevSummaryTaskId")]
        private SummaryTask prevSummaryTask { get; set; }

        public int? PreviousId
        {
            get
            {
                return prevTaskId == null ? prevSummaryTaskId : prevTaskId;
            }
        }
        public IGauntItem Previous
        {
            get
            {
                return prevTask == null ? (IGauntItem)prevSummaryTask : prevTask;
            }
        }

        public void SetPrevious(IGauntItem parent)
        {
            switch (parent)
            {
                case _Task task:
                    {
                        prevTaskId = task.DescriptionId;
                        break;
                    }
                case SummaryTask summaryTask:
                    {
                        prevSummaryTaskId = summaryTask.DescriptionId;
                        break;
                    }
            }
        }
    }
}
