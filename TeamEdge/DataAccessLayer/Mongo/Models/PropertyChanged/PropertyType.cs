namespace TeamEdge.DAL.Mongo.Models
{
    public enum PropertyType : byte
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