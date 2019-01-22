using System;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public class EnumChangedFactory : IHistoryRecordProduser
    {
        public EnumChangedFactory(PropertyType type)
        {
            _type = type;
        }

        private PropertyType _type;

        public PropertyChanged CreateHistoryRecord(object previous, object next)
        {
            return new EnumValueChanged
            {
                Previous = previous,
                New = next,
                Type = _type
            };
        }
    }
}
