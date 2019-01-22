using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IHistoryService
    {
        Task<string> SaveChangeReport(IEnumerable<PropertyChanged> records);
    }
}
