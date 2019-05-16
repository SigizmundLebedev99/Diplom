using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class UpdateSprintDTO
    {
        public int ProjectId { get; set; }
        public int Number { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
        public int[] Tasks { get; set; }
        public int[] UserStories { get; set; }
    }
}
