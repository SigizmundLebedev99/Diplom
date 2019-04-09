using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class SummaryTask : BaseWorkItem, ITimeConstraint
    {
        public override string Code => WorkItemType.SummaryTask.Code();
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public SummaryTask Parent { get; set; }

        public virtual ICollection<SummaryTask> SummaryTaskChildren { get; set; }
        public virtual ICollection<_Task> Children { get; set; }
        public int? ParentSummaryTaskId { get; internal set; }
        public SummaryTask ParentSummaryTask { get; set; }

        public int? GauntPreviousId { get; set; }
        [ForeignKey("GauntPreviousId")]
        public WorkItemDescription GauntPrevious { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? Duration { get; set; }
    }
}
