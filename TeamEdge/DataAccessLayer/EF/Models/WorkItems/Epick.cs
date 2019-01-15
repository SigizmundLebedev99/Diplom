using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class Epick : BaseWorkItem<Feature>
    {
        public override string Code => WorkItemType.Epick.Code();
    }
}