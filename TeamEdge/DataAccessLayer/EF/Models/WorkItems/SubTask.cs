using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class SubTask : BaseWorkItem, IBaseWorkItemWithParent<_Task>
    {
        public int? AssignedToId { get; set; }
        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public _Task Parent { get; set; }

        public override string Code => WorkItemType.SubTask.Code();
    }
}
