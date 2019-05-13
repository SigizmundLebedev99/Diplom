using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.Models;

namespace TeamEdge
{
    public enum WorkItemType : byte
    {
        [WorkItem("EPIC", typeof(EpicRepository), typeof(CreateEpickDTO))]
        Epic,

        [WorkItem("STORY", typeof(UserStoryRepository), typeof(CreateUserStoryDTO))]
        UserStory,

        [WorkItem("SUBTASK", typeof(SubTaskRepository), typeof(CreateSubTaskDTO))]
        SubTask
    }
}
