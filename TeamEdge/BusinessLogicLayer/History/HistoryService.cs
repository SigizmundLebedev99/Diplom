using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;
using TeamEdge.WebLayer;

namespace TeamEdge.BusinessLogicLayer.History
{
    public class HistoryService : IHistoryService
    {
        readonly TeamEdgeDbContext _context;
        readonly IMongoContext _mongo;
        readonly HttpContext _httpContext;

        public HistoryService(TeamEdgeDbContext context, IMongoContext mongo, HttpContext httpContext)
        {
            _context = context;
            _mongo = mongo;
            _httpContext = httpContext;
        }

        public async void CompareForChanges<T>(T previous, T next) where T: BaseWorkItem
        {
            var changes = GetChanges(previous, next);

            var record = new WorkItemChanged
            {
                Changes = changes,
                Code = previous.Code,
                DateOfCreation = next.Description.LastUpdate.Value,
                Number = previous.Number,
                ProjectId = previous.Description.ProjectId,
                Initiator = _httpContext.User.Model()
            };
            _mongo.HistoryRecords.InsertOne(record);
        }

        private List<IPropertyChanged> GetChanges(object obj1, object obj2)
        {
            var t = obj1.GetType();
            if (t != obj2.GetType())
                throw new InvalidOperationException("GetChanges throws");
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<IPropertyChanged> historyRecords = new List<IPropertyChanged>(properties.Length);
            foreach (var prop in properties)
            {
                if (!Attribute.IsDefined(prop, typeof(PropertyChangesAttribute)))
                {
                    if(!prop.PropertyType.IsPrimitive && prop.PropertyType.Assembly == Assembly.GetCallingAssembly())
                    {
                        historyRecords.AddRange(GetChanges(prop.GetValue(obj1), prop.GetValue(obj2)));
                    }
                    continue;
                }

                var attribute = (PropertyChangesAttribute)prop.GetCustomAttribute(typeof(PropertyChangesAttribute));
                var type = attribute.HistoryRecordFactory;
                IHistoryRecordProduser produser = (IHistoryRecordProduser)Activator.CreateInstance(type, prop.Name);
                var result = produser.CreateHistoryRecord(prop.GetValue(obj1), prop.GetValue(obj2));
                if (result != null)
                    historyRecords.Add(result);
            }

            return historyRecords;
        }
    }
}
