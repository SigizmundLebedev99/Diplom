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

    
}
