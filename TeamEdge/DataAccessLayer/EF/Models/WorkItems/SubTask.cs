using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class SubTask : BaseWorkItem, IBaseWorkItemWithParent<_Task>
    {
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public _Task Parent { get; set; }
        public override string Code => WorkItemType.SubTask.Code();
    }
}
