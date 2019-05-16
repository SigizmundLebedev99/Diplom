using System;

namespace TeamEdge.Models
{
    public class CreateTaskDTO : CreateWorkItemDTO
    {
        public int? AssignedToId { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EpicId { get; set; }
    }
}
