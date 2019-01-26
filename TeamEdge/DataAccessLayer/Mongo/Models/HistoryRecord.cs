using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using TeamEdge.Models;

namespace TeamEdge.DAL.Mongo.Models
{
    public abstract class HistoryRecord
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public int ProjectId { get; set; }
        public UserDTO Initiator { get; set; }
    }
}
