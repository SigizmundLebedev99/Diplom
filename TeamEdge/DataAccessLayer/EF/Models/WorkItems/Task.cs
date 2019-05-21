using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public class _Task : BaseWorkItem, IBaseWorkItemWithParent<UserStory>, IBaseWorkItemWithChild<SubTask>, IBaseWorkItemWithParent<Epic>
    {
        [PropertyChanges(typeof(ChildrenChangeFactory))]
        public ICollection<SubTask> Children { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [PropertyChanges(typeof(ParentChangeFactory))]
        public UserStory Parent { get; set; }
        public int? AssignedToId { get; set; }
        public TaskType Type { get; set; }

        public int? EpicId { get; set; }
        [ForeignKey("EpicId")]
        [PropertyChanges(typeof(ParentChangeFactory))]
        public Epic Epic { get; set; }

        public WorkItemStatus Status { get; set; }

        [PropertyChanges(typeof(AssignedToChangeFactory))]
        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }

        public override string Code => Type.Code();

        public int? SprintId { get; set; }

        [ForeignKey("SprintId")]
        public Sprint Sprint { get; set; }

        [NotMapped]
        Epic IBaseWorkItemWithParent<Epic>.Parent { get => Epic; set => Epic = value; }
        [NotMapped]
        int? IBaseWorkItemWithParent<Epic>.ParentId { get => EpicId; set => EpicId = value; }
    }
}
