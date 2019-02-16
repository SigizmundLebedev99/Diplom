using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Helpers
{
    public static class TimeHelper
    {
        public static short ToInt16(this TimeSpan ts)
        {
            var bytes = new byte[]
            {
                (byte)ts.Days,
                (byte)ts.Hours,
            };
            return BitConverter.ToInt16(bytes);
        }

        public static TimeSpan ToTimeSpan(this short number)
        {
            var bytes = BitConverter.GetBytes(number);
            var days = bytes[0];
            var hours = bytes[1];
            return new TimeSpan(days, hours, 0, 0);
        }
    }
}
