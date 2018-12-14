namespace TeamEdge.Models
{
    public struct InviteDTO
    {
        public int FromId { get; set; }
        public string FromFullName { get; set; }
        public string FromEmail { get; set; }
        public string Email { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
    }
}
