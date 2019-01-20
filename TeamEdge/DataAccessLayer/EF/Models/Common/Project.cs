using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamEdge.DAL.Models
{
    public class Project : BaseEntity
    {
        [StringLength(64, MinimumLength =3)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Logo { get; set; }
        public ICollection<UserProject> Users { get; set; }
        public ICollection<WorkItemDescription> WorkItemDescriptions { get; set; }
    }
}
