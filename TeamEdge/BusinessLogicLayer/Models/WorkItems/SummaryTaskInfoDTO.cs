using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.Models;

namespace TeamEdge.Models
{
    public class SummaryTaskInfoDTO : WorkItemDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
