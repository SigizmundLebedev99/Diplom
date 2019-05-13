using System.Collections.Generic;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class Epic : BaseWorkItem, IBaseWorkItemWithChild<UserStory>
    {
        [PropertyChanges(typeof(ChildrenChangeFactory))]
        public ICollection<UserStory> Children { get; set; }
        public ICollection<_Task> Links { get; set; }
        public override string Code => WorkItemType.Epic.Code(); 
    }
}
