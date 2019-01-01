using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;

namespace TeamEdge.Models
{
    public class UserStoryInfoDTO : WorkItemDTO
    {
        public Priority Priority { get; set; }
        public Priority Risk { get; set; }
        public string AcceptenceCriteria { get; set; }
        public string AcceptenceCriteriaCode { get; set; }
        public int? SprintId { get; set; }
        public string SprintName { get; set; }
    }
}
