using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.Models
{
    public class TaskInfoDTO : WorkItemDTO, ITimeConstraint
    {
        public UserDTO AssignedTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ItemDTO Epick { get; set; }
        public int? SprintId { get; set; }
        public int? SprintNumber { get; set; }
        public WorkItemStatus Status { get; set; }
    }
}
