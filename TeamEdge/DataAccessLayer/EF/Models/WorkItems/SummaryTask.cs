using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Helpers;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Models
{
    public class SummaryTask : BaseWorkItem, IBaseWorkItemWithChild<_Task>, IBaseWorkItemWithChild<SummaryTask>, IBaseWorkItemWithParent<SummaryTask>, ITimeConstraint, IGauntItem
    {
        public override string Code => WorkItemType.SummaryTask.Code();

        public ICollection<_Task> Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int? ParentId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [ForeignKey("ParentId")]
        public SummaryTask Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ICollection<SummaryTask> IBaseWorkItemWithChild<SummaryTask>.Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<IBaseWorkItemWithParent<SummaryTask>> AllChildren
        {
            get
            {
                return Children.Concat((IEnumerable<IBaseWorkItemWithParent<SummaryTask>>)((IBaseWorkItemWithChild<SummaryTask>)this).Children);
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return AllChildren.Select(e => ((ITimeConstraint)e).StartDate).Min();
            }
            set { }
        }
        public DateTime? EndDate
        {
            get
            {
                return AllChildren.Select(e => ((ITimeConstraint)e).EndDate).Max();
            }
            set { }
        }
        public short? Duration { get { return (EndDate - StartDate).ToInt16(); } set { } }

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
