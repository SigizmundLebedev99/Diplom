using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class Sprint : BaseEntity
    {
        [StringLength(64, MinimumLength = 3)]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public ICollection<UserStory> UserStories { get; set; }
        public ICollection<_Task> Tasks { get; set; }
    }
}
