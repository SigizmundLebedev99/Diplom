using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class Subscribe
    {
        public int SubscriberId { get; set; }
        [ForeignKey("SubscriberId")]
        public User Subscriber { get; set; }

        public int WorkItemId { get; set; }
        [ForeignKey("WorkItemId")]
        public WorkItemDescription WorkItem { get; set; }
    }
}
