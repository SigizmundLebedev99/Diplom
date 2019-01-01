﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class UserStory : BaseWorkItem<_Task, Feature>
    {
        public Priority Priority { get; set; }
        public Priority Risk { get; set; }
        public string AcceptenceCriteria { get; set; }
        public string AcceptenceCriteriaCode { get; set; }
        public int? SprintId { get; set; }
        [ForeignKey("SprintId")]
        public Sprint Sprint { get; set; }
    }
}
