using TeamEdge.Models;

namespace TeamEdge.Models
{
    public class CreateHistoryRecordDTO
    {
        public int ProjectId { get; set; }
        public UserDTO Initiator { get; set; }
    }
}
