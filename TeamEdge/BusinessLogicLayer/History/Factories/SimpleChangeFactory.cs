using System;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public class SimpleChangeFactory : IHistoryRecordProduser
    {
        public SimpleChangeFactory(string type)
        {
            _type = type;
        }

        private string _type;

        public IPropertyChanged CreateHistoryRecord(object previous, object next)
        {
            return new SimpleValueChanged
            {
                Previous = previous,
                New = next,
                Type = _type
            };
        }
    }
}
