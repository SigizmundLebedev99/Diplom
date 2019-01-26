using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IHistoryService
    {
        void CompareForChanges<T>(T previous, T next) where T : BaseWorkItem;
    }
}
