using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int? RepositoryId { get; set; }
        [ForeignKey("RepositoryId")]
        public Repository Repository { get; set; }
    }
}
