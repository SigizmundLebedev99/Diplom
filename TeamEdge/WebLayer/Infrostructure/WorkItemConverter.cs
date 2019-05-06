using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.Models;

namespace TeamEdge.WebLayer
{
    public class WorkItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var code = obj.Property("code")?.Value.ToString();
            if(code == null)
                throw new NotFoundException("code_nf");
            var type = GetDeserializationType(code);
            object instance = Activator.CreateInstance(type);
            var props = type.GetTypeInfo().GetProperties().ToList();
            foreach(var prop in obj.Properties())
            {
                PropertyInfo info = props.FirstOrDefault(pi => pi.CanWrite && pi.Name.ToUpper() == prop.Name.ToUpper());
                if(info!=null)
                    info.SetValue(instance, prop.Value.ToObject(info.PropertyType, serializer));
            }

            return instance;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static Type GetDeserializationType(string code)
        {
            var attr = WorkItemFactory.GetAttributeInstanse(code);
            if (attr == null)
                throw new NotFoundException("code_inv");
            return attr.DeserializationType;
        }
    }
}
