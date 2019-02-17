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

        public ICollection<_Task> Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int? ParentId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [ForeignKey("ParentId")]
        public SummaryTask Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ICollection<SummaryTask> IBaseWorkItemWithChild<SummaryTask>.Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IQueryable<BaseWorkItem> AllChildren
        {
            get
            {
                return Children.Concat((IEnumerable<BaseWorkItem>)((IBaseWorkItemWithChild<SummaryTask>)this).Children).AsQueryable();
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
    }
}
