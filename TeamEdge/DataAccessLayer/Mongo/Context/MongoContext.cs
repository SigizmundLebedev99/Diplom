using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.DAL.Mongo
{
    public class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; }

        public IMongoCollection<HistoryRecord> HistoryRecords { get; }

        public MongoContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetValue<string>("Mongo:ConnStr"));
            Database = client.GetDatabase(config.GetValue<string>("Mongo:Database"));
            HistoryRecords = Database.GetCollection<HistoryRecord>(config.GetValue<string>("Mongo:HistoryCollection"));
        }

        static MongoContext()
        {
            BsonClassMap.RegisterClassMap<SimpleValueChanged>();
            BsonClassMap.RegisterClassMap<CollectionChanged>();
            BsonClassMap.RegisterClassMap<FileLightDTO>();
            BsonClassMap.RegisterClassMap<UserLightDTO>();
            BsonClassMap.RegisterClassMap<ItemDTO>();
        }
    }
}
