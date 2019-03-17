using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class SubTask : BaseWorkItem, IBaseWorkItemWithParent<_Task>
    {
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public _Task Parent { get; set; }
        public override string Code => WorkItemType.SubTask.Code();
        public int? AssignedToId { get; set; }

        [PropertyChanges(typeof(AssignedToChangeFactory))]
        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }

        public float PersentOfWork { get; set; }
    }
}
