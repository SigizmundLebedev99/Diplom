using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.Models;
using TeamEdge.WebLayer.Infrastructure;

namespace TeamEdge
{
    public enum WorkItemType : byte
    {
        [WorkItem("EPICK", typeof(EpickRepository))]
        [Deserialize("EPICK", typeof(CreateWorkItemDTO))]
        Epick,

        [WorkItem("FEATURE", typeof(FeatureRepository))]
        [Deserialize("FEATURE", typeof(CreateWorkItemDTO))]
        Feature,

        [WorkItem("STORY", typeof(UserStoryRepository))]
        [Deserialize("STORY", typeof(CreateUserStoryDTO))]
        UserStory
    }
}
