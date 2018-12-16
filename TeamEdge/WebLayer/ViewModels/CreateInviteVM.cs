using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public struct CreateInviteVM
    {
        [Required]
        public string Email { get; set; }
        public int ProjectId { get; set; }
    }
}
