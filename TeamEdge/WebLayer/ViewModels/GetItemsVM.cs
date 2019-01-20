using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class GetItemsVM
    {
        [RegularExpression("^[a-zA-Z]{0,8}$")]
        public string Code { get; set; }

        public int? ParentId { get; set; }
        public bool HasNoParent { get; set; }
    }
}
