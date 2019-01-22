using System.Collections.Generic;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class Epick : BaseWorkItem, IBaseWorkItemWithChild<Feature>
    {
        public ICollection<Feature> Children { get; set; }

        public override string Code => WorkItemType.Epick.Code(); 
    }
}