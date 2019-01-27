using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.DAL.Mongo
{
    public class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; }

        private IMongoCollection<HistoryRecord> historyRecords;
        private IMongoCollection<TestCase> testCases;

        public MongoContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetValue<string>("Mongo:ConnStr"));
            Database = client.GetDatabase(config.GetValue<string>("Mongo:Database"));
        }

        public IMongoCollection<HistoryRecord> HistoryRecords { get => historyRecords ?? (historyRecords = Database.GetCollection<HistoryRecord>("historyrecords")); } 
        public IMongoCollection<TestCase> TestCases { get => testCases ?? (testCases = Database.GetCollection<TestCase>("testcases")); }

        
    }
}
