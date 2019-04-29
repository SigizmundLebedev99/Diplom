using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;

namespace TeamEdge.DAL.Models
{
    public class Sprint : BaseEntity, ITimeConstraint
    {
        public int Number { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<UserStory> UserStories { get; set; }
        public virtual ICollection<_Task> Tasks { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
