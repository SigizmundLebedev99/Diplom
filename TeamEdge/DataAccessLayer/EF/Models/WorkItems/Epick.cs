using System.Collections.Generic;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class Epick : BaseWorkItem, IBaseWorkItemWithChild<UserStory>
    {
        [PropertyChanges(typeof(ChildrenChangeFactory))]
        public ICollection<UserStory> Children { get; set; }

        public override string Code => WorkItemType.Epick.Code(); 
    }
}
