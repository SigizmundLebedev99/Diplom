using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class _Task : BaseWorkItem<_Task, UserStory>
    {
        public int? AssignedToId { get; set; }
        public TaskType Type { get; set; }

        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }

        public override string Code => Type.Code();
    }
}
