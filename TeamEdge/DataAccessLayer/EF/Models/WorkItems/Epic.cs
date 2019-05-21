using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class Epic : BaseWorkItem, IBaseWorkItemWithChild<UserStory>, IBaseWorkItemWithChild<_Task>
    {
        [PropertyChanges(typeof(ChildrenChangeFactory))]
        public ICollection<UserStory> Children { get; set; }
        public ICollection<_Task> Links { get; set; }
        public override string Code => WorkItemType.Epic.Code();

        [NotMapped]
        ICollection<_Task> IBaseWorkItemWithChild<_Task>.Children { get => Links; set => Links = value; }
    }
}
