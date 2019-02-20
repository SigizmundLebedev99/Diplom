using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Models.WorkItems.Create
{
    public class CreateSummaryTaskDTO : CreateWorkItemDTO
    {
        public int? SummaryTaskId { get; set; }
    }
}
