using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;

namespace TeamEdge.BusinessLogicLayer.Helpers
{
    static class TimeHelper
    {
        public static short? ToInt16(this TimeSpan? ts)
        {
            if (ts == null)
                return null;

            var bytes = new byte[]
            {
                (byte)ts.Value.Days,
                (byte)ts.Value.Hours,
            };
            return BitConverter.ToInt16(bytes);
        }

        public static TimeSpan? ToTimeSpan(this short? num)
        {
            if (num == null)
                return null;
            var number = num.Value;
            var bytes = BitConverter.GetBytes(number);
            var days = bytes[0];
            var hours = bytes[1];
            return new TimeSpan(days, hours, 0, 0);
        }

        public static short? GetTimeSpanNumber(int? days, int? hours)
        {
            if (days == null && hours == null)
                return null;

            days = days == null ? 0 : days;
            hours = hours == null ? 0 : hours;

            if(hours >= 24)
            {
                days += hours / 24;
                hours = hours % 24;
            }
            var bytes = new byte[]
            {
                (byte)days,
                (byte)hours,
            };
            return BitConverter.ToInt16(bytes);
        }

        public static bool CheckTimeConstraints(ITimeConstraint obj)
        {
            if(obj.StartDate!=null && obj.EndDate != null && obj.Duration != null)
            {
                if (obj.StartDate > obj.EndDate || obj.StartDate == obj.EndDate)
                    return false;
                if (obj.Duration.ToTimeSpan() != obj.EndDate - obj.StartDate)
                    return false;
            }
            else if(obj.StartDate != null && obj.EndDate != null && obj.Duration == null)
            {
                if (obj.StartDate > obj.EndDate || obj.StartDate == obj.EndDate)
                    return false;
                obj.Duration = (obj.EndDate.Value - obj.StartDate.Value).ToInt16();
            }
            else if(obj.StartDate != null && obj.EndDate == null && obj.Duration != null)
            {
                obj.EndDate = obj.StartDate + obj.Duration.ToTimeSpan();
            }
            else if (obj.StartDate == null && obj.EndDate != null && obj.Duration != null)
            {
                obj.StartDate = obj.EndDate - obj.Duration.ToTimeSpan();
            }
            return true;
        }
    }
}
