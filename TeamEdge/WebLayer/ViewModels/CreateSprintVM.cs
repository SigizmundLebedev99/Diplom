using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class CreateSprintVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int[] UserStories { get; set; }
        public int[] Tasks { get; set; }
    }
}
