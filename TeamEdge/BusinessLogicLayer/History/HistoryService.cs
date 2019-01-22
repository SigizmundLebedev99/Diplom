using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.History
{
    public class HistoryService : IHistoryService
    {
        public Task<string> SaveChangeReport(IEnumerable<PropertyChanged> records)
        {
            return Task.Run(() => { return ""; });
        }

        public IEnumerable<PropertyChanged> CompareForChanges<T>(T obj1, T obj2)
        {
            var properties = typeof(T).GetProperties();
            List<PropertyChanged> historyRecords = new List<PropertyChanged>(properties.Length);
            foreach (var prop in properties)
            {
                var attribute = (PropertyChangesAttribute)prop.GetCustomAttribute(typeof(PropertyChangesAttribute));
                if (attribute == null)
                    continue;
                var type = attribute.HistoryRecordFactory;
                IHistoryRecordProduser produser =(IHistoryRecordProduser)(attribute.Type == null ? 
                    Activator.CreateInstance(type) : 
                    Activator.CreateInstance(type, attribute.Type));
                historyRecords.Add(produser.CreateHistoryRecord(prop.GetValue(obj1), prop.GetValue(obj2)));
            }

            return historyRecords;
        }

        private IEnumerable<PropertyChangesAttribute> GetAttributes<T>()
        {
            var type = typeof(T);

            foreach(var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attribute = (PropertyChangesAttribute)prop.GetCustomAttribute(typeof(PropertyChangesAttribute));
                if (attribute != null)
                    yield return attribute;
            }
        }
    }
}
