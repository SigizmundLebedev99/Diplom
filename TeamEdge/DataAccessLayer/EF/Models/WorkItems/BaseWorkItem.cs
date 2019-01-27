using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.DAL.Models
{
    public abstract class BaseWorkItem
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

    public interface IBaseWorkItemWithChild<TChild> where TChild : BaseWorkItem
    {
        ICollection<TChild> Children { get; set; }
    }

    public interface IBaseWorkItemWithParent<TParent> where TParent : BaseWorkItem
    {
        int? ParentId { get; set; }
        TParent Parent { get; set; } 
    }
}
