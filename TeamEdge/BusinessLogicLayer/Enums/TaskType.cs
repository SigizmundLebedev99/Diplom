using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.Models;

namespace TeamEdge
{
    public enum TaskType : byte
    {
        [WorkItem("TASK", typeof(TaskRepository), typeof(CreateTaskDTO))]
        Task,

        [WorkItem("BUG", typeof(TaskRepository), typeof(CreateTaskDTO))]
        Bug,

        [WorkItem("ISSUE", typeof(TaskRepository), typeof(CreateTaskDTO))]
        Issue
    }
}