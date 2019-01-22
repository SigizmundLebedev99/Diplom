using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public interface IHistoryRecordProduser
    {
        PropertyChanged CreateHistoryRecord(object previous, object next);
    }
}
