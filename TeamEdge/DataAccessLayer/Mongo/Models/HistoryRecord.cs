using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TeamEdge.DAL.Mongo.Models
{
    public abstract class HistoryRecord
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public uint WorkItemDescriptionId { get; set; }
        public abstract HistoryRecordType HistoryRecordType { get; }
    }

}
