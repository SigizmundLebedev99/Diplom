using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class Timesheet : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public _Task Task { get; set; }

        [ForeignKey("SubTaskId")]
        public SubTask SubTask { get; set; }
        public int? SubTaskId { get; set; }

        public int? EndedById { get; set; }
        [ForeignKey("EndedById")]
        public User EndedBy { get; set; }
        public WorkItemStatus? EndsWith { get; set; }
    }
}
