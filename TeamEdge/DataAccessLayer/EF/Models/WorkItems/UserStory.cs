using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class UserStory : BaseWorkItem<_Task, Feature>
    {
        public string AcceptenceCriteria { get; set; }
        public string AcceptenceCriteriaJson { get; set; }
        public int? SprintId { get; set; }
        [ForeignKey("SprintId")]
        public Sprint Sprint { get; set; }
    }
}
