using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public abstract class BaseWorkItem
    {
        [Key]
        public int DescriptionId { get; set; }
        [ForeignKey("DescriptionId")]
        public WorkItemDescription Description { get; set; }

        public WorkItemStatus Status { get; set; }
        public int Number { get; set; }

        [StringLength(64, MinimumLength =3)]
        public string Name { get; set; }
        
        public abstract string Code { get; }
    }

    public abstract class BaseWorkItem<TChild> : BaseWorkItem where TChild : BaseWorkItem
    {
        public ICollection<TChild> Children { get; set; }
    }

    public abstract class BaseWorkItem<TChild, TParent> : BaseWorkItem<TChild> where TChild: BaseWorkItem where TParent: BaseWorkItem
    {
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public TParent Parent { get; set; } 
    }
}
