using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Konfik
{
    public static class XDocumentConfigExtensions
    {
        public static string AttributeValue(this XElement xElement, string attribute)
        {
            var xAttribute = xElement.Attribute(attribute);
            if (xAttribute == null) return null;
            return xAttribute.Value;
        }

        public static XElement Element(this XElement xElement, XName xName, Func<XElement, bool> where)
        {
            var xElements = xElement.Elements(xName, @where);
            if (xElements.Count() > 1)
            {
                throw new Exception(string.Format("Found {0} elements <{1}> but expected only 1", xElements.Count(), xName));
            }

            return xElements.SingleOrDefault();
        }

        public static XElement RequireElement(this XElement xElement, XName xName, Func<XElement, bool> where = null)
        {
            var element = xElement.Element(xName, @where ?? ((x) => true));
            if (element == null)
            {
                throw new Exception(string.Format("Found 0 elements <{0}> but expected 1 and only 1", xName));
            }
            return element;
        }

        public static IEnumerable<XElement> Elements(this XElement xElement, XName xName, Func<XElement, bool> where)
        {
            return xElement.Elements(xName).Where(@where);
        }

        public static XElement AppSetting(this XDocument xDocument, string key)
        {
            var matching = xDocument.Root.Element("appSettings").Elements("add").Where(x => x.AttributeValue("key").IgnoreCaseEquals(key));
            if(matching.Count() != 1)
                throw new Exception(string.Format("Found {0} appSettings with key=\"{1}\" but expected 1 and only 1", matching.Count(), key));
            return matching.Single();
        }

        public static XElement ConnectionString(this XDocument xDocument, string name)
        {
            var matching = xDocument.Root.Element("connectionStrings").Elements().Where(x => x.AttributeValue("name").IgnoreCaseEquals(name));
            if (matching.Count() != 1)
                throw new Exception(string.Format("Found {0} connectionStrings with name=\"{1}\" but expected 1 and only 1", matching.Count(), name));
            return matching.Single();
        }

        public static XElement ConfigSectionByType(this XDocument xDocument, string type)
        {
            var configSection = xDocument.Root.Element("configSections");
            var matchingSections = configSection.Elements("section").Where(x => StringExtensions.DontIgnoreCaseEquals(x.Attribute("type").Value, type)).ToList();

            if(matchingSections.Count() != 1)
                throw new Exception(string.Format("Found {0} configSections.section's but expected 1 and only 1 with type \"{1}\"", matchingSections.Count(), type));

            var sectionName = matchingSections.Single().Attribute("name").Value;
            var sections = xDocument.Root.Elements(sectionName).ToList();

            if(sections.Count() != 1)
                throw new Exception(string.Format("Found {0} sections named <{1}> but expected 1 and only 1", sections.Count(), sectionName));

            return sections.Single();
        }
    }
}