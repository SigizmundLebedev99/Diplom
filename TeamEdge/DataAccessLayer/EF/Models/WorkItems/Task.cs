using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class _Task : BaseWorkItem, IBaseWorkItemWithParent<UserStory>, IBaseWorkItemWithChild<SubTask>
    {
        public ICollection<SubTask> Children { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [PropertyChanges(typeof(ParentChangeFactory))]
        public UserStory Parent { get; set; }
        [PropertyChanges(typeof(SimpleChangeFactory))]
        public int? AssignedToId { get; set; }
        public TaskType Type { get; set; }

        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }
        [PropertyChanges(typeof(SimpleChangeFactory))]
        public int? SprintId { get; set; }
        [ForeignKey("SprintId")]
        public Sprint Sprint { get; set; }

        public override string Code => Type.Code();

        
    }
}
