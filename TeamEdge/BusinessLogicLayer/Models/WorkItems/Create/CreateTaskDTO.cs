using System;

namespace TeamEdge.Models
{
    public class CreateTaskDTO : CreateWorkItemDTO
    {
        public int? AssignedToId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int EpickId { get; set; }
    }
}
