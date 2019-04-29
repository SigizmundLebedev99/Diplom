using System;

namespace TeamEdge.BusinessLogicLayer
{
    interface ITimeConstraint
    {
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
    }
}
