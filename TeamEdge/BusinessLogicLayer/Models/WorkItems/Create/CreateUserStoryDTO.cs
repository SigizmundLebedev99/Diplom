namespace TeamEdge.Models
{
    public class CreateUserStoryDTO : CreateWorkItemDTO
    {
        public Priority Priority { get; set; }
        public Priority Risk { get; set; }
        public string AcceptenceCriteria { get; set; }
        public string AcceptenceCriteriaCode { get; set; }
        public int? SprintId { get; set; }
    }
}
