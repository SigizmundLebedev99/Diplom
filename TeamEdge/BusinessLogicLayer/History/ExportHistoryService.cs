using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using TeamEdge.DAL.Mongo;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer
{
    public class ExportHistoryService
    {
        readonly IMongoContext _context;

        public ExportHistoryService(IMongoContext context)
        {
            _context = context;
        }

        public async Task<string> GetHistoryRecordsForProject(int projectId, int skip, int take)
        {
            var res = await _context.HistoryRecords.FindAsync(e => e.ProjectId == projectId, new FindOptions<HistoryRecord, HistoryRecord>()
            {
                Skip = skip,
                Limit = take
            });

            return res.ToJson();
        }

        public async Task<string> GetHistoryRecordsForItem(int projectId, string code, int number, int skip, int take)
        {
            var res = await _context.HistoryRecords.OfType<WorkItemChanged>()
                .FindAsync(e => e.ProjectId == projectId && e.Code == code && e.Number == number, new FindOptions<WorkItemChanged, WorkItemChanged>()
                {
                    Skip = skip,
                    Limit = take
                });
            return res.ToJson();
        }
    }
}
