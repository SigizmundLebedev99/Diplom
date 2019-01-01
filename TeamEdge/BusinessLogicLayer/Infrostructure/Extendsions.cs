using System;
using System.Reflection;
using TeamEdge.BusinessLogicLayer.Infrastructure;

namespace TeamEdge.BusinessLogicLayer
{
    public static class Extendsions
    {
        public static string Code(this WorkItemType wit)
        {
            Type type = wit.GetType();
            return GetCode(type);
        }

        public static string Code(this TaskType tt)
        {
            Type type = tt.GetType();
            return GetCode(type);
        }

        private static string GetCode(Type type)
        {
            MemberInfo[] members = type.GetMember(type.ToString());
            if (members != null && members.Length > 0)
            {
                var attrs = members[0].GetCustomAttribute(typeof(WorkItemAttribute), false);
                if (attrs != null)
                {
                    return ((WorkItemAttribute)attrs).Code;
                }
            }
            throw new NotFoundException("code_nf");
        }
    }
}
