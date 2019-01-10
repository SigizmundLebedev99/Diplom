using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.Models;

namespace TeamEdge
{
    public enum TaskType : byte
    {
        [WorkItem("TASK", typeof(TaskFactory), typeof(CreateTaskDTO))]
        Task,

        [WorkItem("BUG", typeof(TaskFactory), typeof(CreateTaskDTO))]
        Bug,

        [WorkItem("ISSUE", typeof(TaskFactory), typeof(CreateTaskDTO))]
        Issue
    }
}