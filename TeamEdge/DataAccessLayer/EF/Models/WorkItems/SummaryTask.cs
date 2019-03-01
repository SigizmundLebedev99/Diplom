using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Helpers;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Models
{
    public class SummaryTask : BaseWorkItem, IBaseWorkItemWithChild<_Task>, IBaseWorkItemWithChild<SummaryTask>, IBaseWorkItemWithParent<SummaryTask>, ITimeConstraint
    {
        public override string Code => WorkItemType.SummaryTask.Code();
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public SummaryTask Parent { get; set; }

        public ICollection<SummaryTask> SummaryTaskChildren { get; set; }
        public ICollection<_Task> Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int? PreviousTaskId { get; set; }
        public int? PreviousSummaryTaskId { get; set; }
        [ForeignKey("PreviousTaskId")]
        public _Task PreviousTask { get; set; }
        [ForeignKey("PreviousSummaryTaskId")]
        private SummaryTask PreviousSummaryTask { get; set; }

        public int? ParentSummaryTaskId { get; internal set; }
        public SummaryTask ParentSummaryTask { get; set; }

        ICollection<SummaryTask> IBaseWorkItemWithChild<SummaryTask>.Children { get => SummaryTaskChildren; set => SummaryTaskChildren = value; }

        public IEnumerable<IBaseWorkItemWithParent<SummaryTask>> AllChildren
        {
            get
            {
                return Children.Concat((IEnumerable<IBaseWorkItemWithParent<SummaryTask>>)SummaryTaskChildren);
            }
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? Duration { get; set; }
    }
}
