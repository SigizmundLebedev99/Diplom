using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TeamEdge.Models
{
    public class TimesheetDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public WorkItemStatus? EndsWith { get; set; }
        public ItemDTO Subtask { get; set; }
        public UserLightDTO ChangedBy { get; set; }
    }
}
