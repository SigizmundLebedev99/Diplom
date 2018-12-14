using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.DAL.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Text { get; set; }
        public string Json { get; set; }
    }
}
