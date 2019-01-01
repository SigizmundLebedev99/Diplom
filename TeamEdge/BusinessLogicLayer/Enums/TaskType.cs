using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.Models;
using TeamEdge.WebLayer.Infrastructure;

namespace TeamEdge
{
    public enum TaskType : byte
    {
        [WorkItem("TASK", typeof(TaskFactory))]
        [Deserialize("TASK", typeof(CreateTaskDTO))]
        Task,

        [WorkItem("BUG", typeof(TaskFactory))]
        [Deserialize("BUG", typeof(CreateTaskDTO))]
        Bug,

        [WorkItem("ISSUE", typeof(TaskFactory))]
        [Deserialize("ISSUE", typeof(CreateTaskDTO))]
        Issue
    }
}