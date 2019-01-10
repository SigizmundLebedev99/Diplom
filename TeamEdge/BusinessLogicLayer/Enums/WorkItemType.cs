using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.Models;

namespace TeamEdge
{
    public enum WorkItemType : byte
    {
        [WorkItem("EPICK", typeof(EpickRepository), typeof(CreateWorkItemDTO))]
        Epick,

        [WorkItem("FEATURE", typeof(FeatureRepository), typeof(CreateWorkItemDTO))]
        Feature,

        [WorkItem("STORY", typeof(UserStoryRepository), typeof(CreateUserStoryDTO))]
        UserStory
    }
}
