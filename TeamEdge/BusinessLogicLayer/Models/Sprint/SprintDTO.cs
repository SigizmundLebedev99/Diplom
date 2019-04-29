using System;
using System.Collections.Generic;

namespace TeamEdge.Models
{
    public class SprintDTO
    {
        public int ProjectId { get; set; }
        public int Number { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<ItemForBacklogDTO> Children { get; set; }
    }
}
