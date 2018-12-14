using MongoDB.Bson;
using MongoDB.Driver;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.DAL.Mongo
{
    public interface IMongoContext
    {
        IMongoCollection<HistoryRecord> HistoryRecords { get;}
        IMongoCollection<TestCase> TestCases { get; }
        IMongoDatabase Database { get; }
    }
}
