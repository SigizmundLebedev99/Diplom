using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.WebLayer;

namespace TeamEdge.BusinessLogicLayer.History
{
    public class HistoryService : IHistoryService
    {
        readonly IMongoContext _mongo;

        public HistoryService(IMongoContext mongo)
        {
            _mongo = mongo;
        }

        public void CompareForChanges<T>(T previous, T next, ClaimsPrincipal user) where T : BaseWorkItem
        {
            Task.Run(() => 
            {
                try
                {
                    var changes = GetChanges(previous, next);
                    if (changes.Count == 0)
                        return;
                    var record = new WorkItemChanged
                    {
                        Changes = changes,
                        Code = previous.Code,
                        DateOfCreation = next.Description.LastUpdate.Value,
                        Number = previous.Number,
                        ProjectId = previous.Description.ProjectId,
                        Initiator = user.Model(),
                        Name = next.Name
                    };
                    _mongo.HistoryRecords.InsertOne(record);
                }
                catch (Exception ex)
                {

                }
            });
        }

        private List<PropertyChanged> GetChanges(object obj1, object obj2)
        {
            var t = obj1.GetType();
            if (t != obj2.GetType())
                throw new InvalidOperationException("GetChanges throws");
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<PropertyChanged> historyRecords = new List<PropertyChanged>(properties.Length);
            foreach (var prop in properties)
            {
                if (!Attribute.IsDefined(prop, typeof(PropertyChangesAttribute)))
                {
                    if(!prop.PropertyType.IsPrimitive && prop.PropertyType.Assembly == Assembly.GetCallingAssembly())
                    {
                        var par1 = prop.GetValue(obj1);
                        var par2 = prop.GetValue(obj2);
                        if (par1 == null || par2 == null)
                            continue;
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
