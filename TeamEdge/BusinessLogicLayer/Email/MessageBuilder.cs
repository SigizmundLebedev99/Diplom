using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TeamEdge.BusinessLogicLayer.Email
{
    public static class MessageBuilder
    {
        public static string BuildMessageHtml(string path, object dataContext)
        {
            var document = XDocument.Load(path);
            BuildElement(document.Root,dataContext, new Dictionary<string, object>());
            return document.ToString();
        }

        private static void BuildElement(XElement element, object dataContext, Dictionary<string, object> variables)
        {
            var allElements = GetAllElements(element).ToList();
            allElements.Add(element);
            Bind(allElements, dataContext, variables);
            var collections = allElements.Where(e => e.Attribute("for") != null);
            var scopes = allElements.Where(e => e.Attribute("slot-scope") != null);
            if (collections.Count() > 0)
            {
                foreach (var c in collections)
                {
                    BuildCollection(c, dataContext, variables);
                }
            }

            if (scopes.Count() > 0)
            {
                foreach (var t in scopes)
                {
                    BuildSlotScope(t, dataContext, variables);
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
                if (el.Attribute("for") != null || el.Attribute("slot-scope") != null)
                    continue;
                var grantchilds = GetAllElements(el);
                addedElements.AddRange(grantchilds);
            }
            return elements.Concat(addedElements);
        }

        private static void Bind(IEnumerable<XElement> elements, object dataContext, Dictionary<string, object> variables)
        {
            var reg = new Regex("{{(.*?)}}");
            var mustashes = elements.Where(e => reg.IsMatch(e.Value) && !e.HasElements);
            var attributes = elements.SelectMany(e => e.Attributes().Where(a => reg.IsMatch(a.Value)));
            foreach (var m in mustashes)
            {
                var matches = reg.Matches(m.Value);
                foreach (Match match in matches)
                {
                    var binding = match.Groups[1].Value;
                    object value = GetBindingValue(binding, dataContext, variables);
                    m.Value = value == null ? m.Value : reg.Replace(m.Value,
                        new MatchEvaluator((mh) =>
                        {
                            if (mh.Index == match.Index)
                                return value.ToString();
                            else
                                return mh.Value;

                        })
                    );
                }
            }

            foreach (var m in attributes)
            {
                var matches = reg.Matches(m.Value);
                foreach (Match match in matches)
                {
                    var binding = match.Groups[1].Value;
                    object value = GetBindingValue(binding, dataContext, variables);
                    m.Value = value == null ? m.Value : reg.Replace(m.Value,
                        new MatchEvaluator((mh) =>
                        {
                            if (mh.Value == match.Value)
                                return value.ToString();
                            else
                                return mh.Value;
                        })
                    );
                }
            }
        }

        private static void BuildCollection(XElement collTrigger, object dataContext, Dictionary<string, object> variables)
        {
            var attr = collTrigger.Attribute("for");
            attr.Remove();
            var attrValue = attr.Value;
            string[] strings = attrValue.Split(' ').ToArray();
            string variable = strings[0];
            string bindingStr = strings[2];
            if (strings.Length != 3 && strings[1] != "in")
                throw new ArgumentException("Collection notation invalid");

            object value = GetBindingValue(bindingStr, dataContext, variables);
            var collection = value as IEnumerable<object>;
            if (collection == null)
            {
                collTrigger.Remove();
                return;
            }

            foreach (var obj in collection)
            {
                if (!variables.TryGetValue(variable, out var previous))
                    variables.Add(variable, obj);
                else
                    variables[variable] = obj;

                var element = new XElement(collTrigger);
                BuildElement(element, dataContext, variables);
                collTrigger.AddAfterSelf(element);
            }
            collTrigger.Remove();
            variables.Remove(variable);
        }

        private static void BuildSlotScope(XElement scopeElement, object dataContext, Dictionary<string, object> variables)
        {
            var slots = scopeElement.Elements().Where(e => e.Attribute("slot") != null).ToLookup(e => e.Attribute("slot").Value);
            if (slots.Count() == 0)
                return;

            var attr = scopeElement.Attribute("slot-scope");
            attr.Remove();
            string type = GetBindingValue(attr.Value, dataContext, variables).GetType().Name;
            if (slots[type] != null && slots[type].Count() == 0)
                type = "~";
            if (slots[type] == null)
                return;
            foreach (var slot in slots[type])
            {
                slot.Attribute("slot").Remove();
                BuildElement(slot, dataContext, variables);
            }

            foreach (var s in slots.Where(e => e.Key != type))
                s.Remove();

            var elements = GetAllElements(scopeElement);
            Bind(elements, dataContext, variables);
        }

        private static object GetBindingValue(string binding, object dataContext, Dictionary<string, object> variables)
        {
            var strings = binding.Split('.');
            if (variables.TryGetValue(strings[0], out var value))
                binding = binding.Replace(strings.Length > 1 ? strings[0] + "." : strings[0], "");
            else
                value = dataContext;
            if (!string.IsNullOrEmpty(binding))
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
