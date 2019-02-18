using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;

namespace TeamEdge.BusinessLogicLayer.Helpers
{
    public interface IGauntItem : IBaseWorkItem
    {
        int? PreviousId { get; }
        IGauntItem Previous { get; }
        void SetPrevious(IGauntItem parent);
    }
}
