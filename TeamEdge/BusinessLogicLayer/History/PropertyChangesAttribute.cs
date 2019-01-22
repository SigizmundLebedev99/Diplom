using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public class PropertyChangesAttribute : Attribute
    {
        public Type HistoryRecordFactory { get; set; }
        public object Type { get; set; }

        public PropertyChangesAttribute(Type type)
        {
            HistoryRecordFactory = type;
        }
    }
}
