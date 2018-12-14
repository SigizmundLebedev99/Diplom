using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Mongo.Models
{
    public abstract class EnumValueChanged<TEnum> : HistoryRecord where TEnum : Enum
    {
        public TEnum Previous { get; set; }

        public TEnum New { get; set; }
    }

    public class StatusChanged : EnumValueChanged<WorkItemStatus>
    {
        public override HistoryRecordType HistoryRecordType => HistoryRecordType.StatusChanged;
    }

    public class PriorityChanged : EnumValueChanged<Priority>
    {
        public override HistoryRecordType HistoryRecordType => HistoryRecordType.PriorityChanged;
    }

    public class RiskChanged : EnumValueChanged<Priority>
    {
        public override HistoryRecordType HistoryRecordType => HistoryRecordType.RiskChanged;
    }


}
