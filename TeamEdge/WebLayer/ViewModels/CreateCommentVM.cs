using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class CreateCommentVM
    {
        public string Text { get; set; }
        public int WorkItemId { get; set; }
        public int[] Files { get; set; }
    }
}
