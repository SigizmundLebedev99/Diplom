using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge
{
    public static class WorkItemFactory
    {
        static WorkItemFactory()
        {
            EnumElements = typeof(WorkItemType)
                    .GetMembers()
                    .Select(e => new WorkItemSlot((WorkItemAttribute)e.GetCustomAttribute(typeof(WorkItemAttribute)), e.Name))
                .Concat(typeof(TaskType)
                    .GetMembers()
                    .Select(e => new WorkItemSlot((WorkItemAttribute)e.GetCustomAttribute(typeof(WorkItemAttribute)), e.Name)))
                    .Where(e => e.Attribute != null)
                    .ToArray();
        }

        public static WorkItemAttribute GetAttributeInstanse(string code)
        {
            return EnumElements.Select(e=>e.Attribute).FirstOrDefault(e => code.StartsWith(e.Code));
        }

        public static string GetEnumElement(string code)
        {
            return EnumElements.FirstOrDefault(e => e.Attribute.Code == code).EnumElement;
        }

        private static IEnumerable<WorkItemSlot> EnumElements;
    }

    class WorkItemSlot
    {
        public WorkItemAttribute Attribute { get; set; }
        public string EnumElement { get; set; }

        public WorkItemSlot(WorkItemAttribute attr, string element)
        {
            Attribute = attr;
            EnumElement = element;
        }
    }
}
