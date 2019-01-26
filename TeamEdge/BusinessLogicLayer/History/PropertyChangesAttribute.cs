using System;
namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public class PropertyChangesAttribute : Attribute
    {
        public Type HistoryRecordFactory { get; set; }

        public PropertyChangesAttribute(Type type)
        {
            HistoryRecordFactory = type;
        }
    }
}
