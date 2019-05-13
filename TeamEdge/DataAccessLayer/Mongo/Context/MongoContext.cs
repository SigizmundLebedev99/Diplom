using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.DAL.Mongo
{
    public class MongoContext : IMongoContext
    {
        private IConfiguration _config;

        public IMongoDatabase Database { get; }

        private IMongoCollection<HistoryRecord> historyRecords;

        public IMongoCollection<HistoryRecord> HistoryRecords { get => historyRecords ??
                (historyRecords = Database.GetCollection<HistoryRecord>(_config.GetValue<string>("Mongo:HistoryCollection"))); }

        public MongoContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetValue<string>("Mongo:ConnStr"));
            Database = client.GetDatabase(config.GetValue<string>("Mongo:Database"));
            _config = config;
        }
    }
}
