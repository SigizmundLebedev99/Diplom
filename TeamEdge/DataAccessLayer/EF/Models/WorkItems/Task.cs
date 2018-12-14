using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class Task : BaseWorkItem<Task, UserStory>
    {
        public int AssignedToId { get; set; }
        public TaskType Type { get; set; }

        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }
    }
}
