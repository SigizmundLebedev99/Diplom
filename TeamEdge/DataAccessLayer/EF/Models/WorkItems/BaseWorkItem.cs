using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.DAL.Models
{
    public abstract class BaseWorkItem : IBaseWorkItem
    {
        [Key]
        public int DescriptionId { get; set; }
        [ForeignKey("DescriptionId")]
        public WorkItemDescription Description { get; set; }

        [PropertyChanges(typeof(SimpleChangeFactory))]
        public WorkItemStatus Status { get; set; }
        public int Number { get; set; }
        [PropertyChanges(typeof(SimpleChangeFactory))]
        [StringLength(64, MinimumLength =3)]
        public string Name { get; set; }
        
        public abstract string Code { get; }
    }

    public interface IBaseWorkItem
    {
        int DescriptionId { get; set; }
        WorkItemDescription Description { get; set; }

        WorkItemStatus Status { get; set; }
        int Number { get; set; }
        string Name { get; set; }

        string Code { get; }
    }

    public interface IBaseWorkItemWithChild<TChild> : IBaseWorkItem where TChild : IBaseWorkItem
    {
        ICollection<TChild> Children { get; set; }
    }

    public interface IBaseWorkItemWithParent : IBaseWorkItem
    {
        int? ParentId { get; set; }
    }

    public interface IBaseWorkItemWithParent<TParent> : IBaseWorkItemWithParent where TParent : IBaseWorkItem
    {
        TParent Parent { get; set; } 
    }
}
