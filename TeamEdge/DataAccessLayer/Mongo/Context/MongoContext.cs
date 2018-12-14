using MongoDB.Driver;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.DAL.Mongo
{
    public class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; }

        private IMongoCollection<HistoryRecord> historyRecords;
        private IMongoCollection<TestCase> testCases;

        public MongoContext(string connStr, string databaseName)
        {
            var client = new MongoClient(connStr);
            Database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<HistoryRecord> HistoryRecords { get => historyRecords ?? (historyRecords = Database.GetCollection<HistoryRecord>("historyRecords")); } 
        public IMongoCollection<TestCase> TestCases { get => testCases ?? (testCases = Database.GetCollection<TestCase>("testCases")); }

        
    }
}
