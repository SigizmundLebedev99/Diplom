using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class Feature : BaseWorkItem<UserStory, Epick>
    {
        public override string Code => WorkItemType.Feature.Code();
    }
}
