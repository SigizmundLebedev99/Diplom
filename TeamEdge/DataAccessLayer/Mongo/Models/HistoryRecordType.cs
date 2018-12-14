namespace TeamEdge.DAL.Mongo.Models
{
    public enum HistoryRecordType : byte
    {
        StatusChanged,
        PriorityChanged,
        DescriptionChanged,
        RiskChanged,
        AssignedPersonChanged,
        LinkChanged,
        TimeChanged
    }
}