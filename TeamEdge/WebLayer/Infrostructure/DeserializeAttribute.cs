using System;

namespace TeamEdge.WebLayer.Infrastructure
{
    public class DeserializeAttribute : Attribute
    {
        public string Code { get; set; }
        public Type Type { get; set; }

        public DeserializeAttribute(string code, Type type)
        {
            Type = type;
            Code = code;
        }
    }
}
