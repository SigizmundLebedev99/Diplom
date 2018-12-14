namespace TeamEdge.DAL.Models
{
    public class BranchLink : BaseEntity
    {
        public string Repository { get; set; }
        public string Branch { get; set; }
    }
}
