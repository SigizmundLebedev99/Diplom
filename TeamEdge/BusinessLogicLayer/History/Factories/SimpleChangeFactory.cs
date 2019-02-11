using System;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    class SimpleChangeFactory : IHistoryRecordProduser
    {
        public SimpleChangeFactory(string type)
        {
            _type = type;
        }

        private string _type;

        public IPropertyChanged CreateHistoryRecord(object previous, object next)
        {
            var p = previous.ToString();
            var n = next.ToString();

            if(p != n)
                return new SimpleValueChanged
                {
                    Previous = previous,
                    New = next,
                    PropertyName = _type
                };

            return null;
        }
    }
}
