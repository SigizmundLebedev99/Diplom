using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace TeamEdge.BusinessLogicLayer.Email
{
    public static class MessageBuilder
    {
        public static string BuildMessageHtml(string path, object dataContext)
        {
            var document = XDocument.Load(path);
            BuildElement(document.Root,dataContext);
            return document.ToString();
        }

        private static void BuildElement(XElement element, object dataContext)
        {
            var allElements = GetAllElements(element);
            Bind(allElements, dataContext);
            var collections = allElements.Where(e => e.Attribute("collection") != null);
            if(collections.Count()>0)
            {
                foreach(var c in collections)
                {
                    BuildCollection(c, dataContext);
                }
            }
        }

        private static IEnumerable<XElement> GetAllElements(XElement element)
        {
            if (!element.HasElements)
                return new XElement[] { };
            var elements = element.Elements();
            var addedElements = new List<XElement>();
            foreach (var el in elements)
            {
                if (el.Attribute("collection") != null)
                    continue;
                var grantchilds = GetAllElements(el);
                addedElements.AddRange(grantchilds);
            }
            return elements.Concat(addedElements);
        }

        private static void Bind(IEnumerable<XElement> elements, object dataContext)
        {
            var mustashes = elements.Where(e => Regex.IsMatch(e.Value, "{{.*?}}") && !e.HasElements);
            var attributes = elements.SelectMany(e => e.Attributes().Where(a => Regex.IsMatch(a.Value, "{{.*?}}")));
            foreach (var m in mustashes)
            {
                var firstMust = m.Value.IndexOf("{{");
                var secondMust = m.Value.IndexOf("}}");
                var binding = m.Value.Substring(firstMust + 2).Remove(secondMust - 2);
                object value = GetBindingValue(binding, dataContext);
                m.Value = value == null ? m.Value : Regex.Replace(m.Value, "{{.*?}}", value.ToString());
            }

            foreach (var m in attributes)
            {
                var firstMust = m.Value.IndexOf("{{");
                var secondMust = m.Value.IndexOf("}}");
                var binding = m.Value.Substring(firstMust + 2).Remove(secondMust - 2);
                object value = GetBindingValue(binding, dataContext);
                m.Value = value == null ? m.Value : Regex.Replace(m.Value, "{{.*?}}", value.ToString());
            }
        }

        private static void BuildCollection(XElement coll, object dataContext)
        {
            var binding = coll.Attribute("collection").Value;
            object value = GetBindingValue(binding, dataContext);
            var collection = value as IEnumerable<object>;
            var slots = coll.Elements().Where(e => e.Attribute("slot") != null).ToLookup(e => e.Attribute("slot").Value);
            foreach(var obj in collection)
            {
                var slot = slots[obj.GetType().Name].First();
                BuildElement(slot, obj);
            }
        }

        private static object GetBindingValue(string binding, object dataContext)
        {
            object value = dataContext;
            foreach (var prop in binding.Split('.'))
            {
                if (value == null)
                    break;
                value = value.GetType().GetProperty(prop)?.GetValue(value);
            }
            return value;
        }
    }
}
