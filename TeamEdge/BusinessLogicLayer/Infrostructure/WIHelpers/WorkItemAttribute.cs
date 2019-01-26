using System;
using TeamEdge.BusinessLogicLayer.Services;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public class WorkItemAttribute : Attribute
    {
        public Type FactoryType { get; set; }
        public Type DeserializationType { get; set; }
        public string Code { get; set; }

        public WorkItemAttribute(string code, Type factoryType, Type deserializationType)
        {
            FactoryType = factoryType;
            DeserializationType = deserializationType;
            Code = code;
        }
    }
}
