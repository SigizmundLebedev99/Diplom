using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class UserStory : BaseWorkItem, IBaseWorkItemWithParent<Feature>, IBaseWorkItemWithChild<_Task>
    {
        public ICollection<_Task> Children { get; set; }

        public int? ParentId { get; set; }

        [PropertyChanges(typeof(ParentChangeFactory))]
        [ForeignKey("ParentId")]
        public Feature Parent { get; set; }

        [PropertyChanges(typeof(SimpleChangeFactory))]
        public Priority Priority { get; set; }

        public string AcceptenceCriteria { get; set; }

        [PropertyChanges(typeof(SimpleChangeFactory))]
        public int? SprintId { get; set; }

        [ForeignKey("SprintId")]
        public Sprint Sprint { get; set; }

        public override string Code => WorkItemType.UserStory.Code();
    }
}
