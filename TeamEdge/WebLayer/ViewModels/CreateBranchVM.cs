using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class CreateBranchVM
    {
        public int ProjectId { get; set; }
        [Required]
        public string Branch { get; set; }
        public string FromSha { get; set; }
    }
}
