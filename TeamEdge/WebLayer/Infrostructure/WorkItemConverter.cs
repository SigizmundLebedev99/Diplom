using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.Models;
using TeamEdge.WebLayer.Infrastructure;

namespace TeamEdge.WebLayer
{
    public class WorkItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(CreateWorkItemDTO) || objectType.IsSubclassOf(typeof(CreateWorkItemDTO)))
                return true;
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string code = null;
            while (reader.Read())
            {
                if (reader.Value != null && reader.Value.ToString().ToUpper() == "CODE")
                {
                    code = reader.ReadAsString();
                    break;
                }
            }
            if (string.IsNullOrEmpty(code))
                throw new NotFoundException("code_nf");

            var type = GetDeserializationType(code);
            var deserialize = serializer.GetType().GetMethod(nameof(serializer.Deserialize));
            deserialize.MakeGenericMethod(type);
            return deserialize.Invoke(serializer, new object[] { reader });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        static WorkItemConverter()
        {
            EnumElements = typeof(WorkItemType)
                    .GetMembers()
                    .Select(e => (DeserializeAttribute)e.GetCustomAttribute(typeof(DeserializeAttribute)))
                .Concat(typeof(TaskType)
                    .GetMembers()
                    .Select(e => (DeserializeAttribute)e.GetCustomAttribute(typeof(DeserializeAttribute))));
        }

        private static IEnumerable<DeserializeAttribute> EnumElements;

        private static Type GetDeserializationType(string code)
        {
            var attr = EnumElements.FirstOrDefault(e => e.Code == code);
            if (attr == null)
                throw new NotFoundException("code_inv");
            return attr.Type;
        }
    }
}
