using System;
using TeamEdge.BusinessLogicLayer.Services;

namespace TeamEdge.BusinessLogicLayer.Infrastructure
{
    public class WorkItemAttribute : Attribute
    {
        public Type Type { get; set; }
        public string Code { get; set; }

        public WorkItemAttribute(string code, Type type)
        {
            if (!type.IsSubclassOf(typeof(WorkItemRepository)))
                throw new InvalidOperationException();
            Type = type;
            Code = code;
        }
    }
}
