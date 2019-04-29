using System;

namespace TeamEdge.Models
{
    public class CreateSprintDTO
    {
        public int ProjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CreatorId { get; set; }
        public int[] Tasks { get; set; }
        public int[] UserStories { get; set; }
    }
}
