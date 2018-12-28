namespace TeamEdge.Models
{
    public struct ProjectInfoDTO
    {
        public UserDTO[] Partisipants { get; set; }
        public int Id { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
    }
}
