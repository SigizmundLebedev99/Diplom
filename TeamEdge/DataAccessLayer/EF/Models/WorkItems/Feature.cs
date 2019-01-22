using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class Feature : BaseWorkItem, IBaseWorkItemWithParent<Epick>, IBaseWorkItemWithChild<UserStory>
    {
        public ICollection<UserStory> Children { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Epick Parent { get; set; }

        public override string Code => WorkItemType.Feature.Code();
    }
}
