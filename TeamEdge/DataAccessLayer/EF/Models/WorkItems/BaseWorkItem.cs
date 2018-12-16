using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class BaseWorkItem : BaseEntity
    {
        public WorkItemStatus Status { get; set; }
        public Priority Priority { get; set; }
        public Priority Risk { get; set; }

        [StringLength(64, MinimumLength =3)]
        public string Name { get; set; }
        public WorkItemDescription Description { get; set; }

        public int LastUpdaterId { get; set; }
        [ForeignKey("LastUpdaterId")]
        public User LastUpdater { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class BaseWorkItem<TChild> : BaseWorkItem where TChild : BaseWorkItem
    {
        public ICollection<TChild> Children { get; set; }
    }

    public class BaseWorkItem<TChild, TParent> : BaseWorkItem<TChild> where TChild: BaseWorkItem where TParent: BaseWorkItem
    {
        public TParent Parent { get; set; } 
    }
}
