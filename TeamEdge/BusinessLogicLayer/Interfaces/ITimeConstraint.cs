using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer
{
    interface ITimeConstraint
    {
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        short? Duration { get; set; }
    }
}
