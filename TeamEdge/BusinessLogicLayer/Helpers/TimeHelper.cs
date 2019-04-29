using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.BusinessLogicLayer.Helpers
{
    static class TimeHelper
    {
        //public static short? ToInt16(this TimeSpan? ts)
        //{
        //    if (ts == null)
        //        return null;

        //    var bytes = new byte[]
        //    {
        //        (byte)ts.Value.Days,
        //        (byte)ts.Value.Hours,
        //    };
        //    return BitConverter.ToInt16(bytes);
        //}

        //public static TimeSpan? ToTimeSpan(this short? num)
        //{
        //    if (num == null)
        //        return null;
        //    var number = num.Value;
        //    var bytes = BitConverter.GetBytes(number);
        //    var days = bytes[0];
        //    var hours = bytes[1];
        //    return new TimeSpan(days, hours, 0, 0);
        //}

        //public static short? GetTimeSpanNumber(int? days, int? hours)
        //{
        //    if (days == null && hours == null)
        //        return null;

        //    days = days == null ? 0 : days;
        //    hours = hours == null ? 0 : hours;

        //    if(hours >= 24)
        //    {
        //        days += hours / 24;
        //        hours = hours % 24;
        //    }
        //    var bytes = new byte[]
        //    {
        //        (byte)days,
        //        (byte)hours,
        //    };
        //    return BitConverter.ToInt16(bytes);
        //}

        public static bool CheckTimeConstraints(this ITimeConstraint obj)
        {
            if (obj.StartDate > obj.EndDate)
                return false;
            return true;
        }
    }
}
