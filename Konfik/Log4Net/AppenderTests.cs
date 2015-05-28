using System;
using System.Xml.Linq;

namespace Konfik.Log4Net
{
    public class AppenderTests
    {
        private readonly Log4NetConfigurationTest log4NetConfigurationTest;

        public AppenderTests(Log4NetConfigurationTest log4NetConfigurationTest, XElement appenderElement)
        {
            this.log4NetConfigurationTest = log4NetConfigurationTest;
            AppenderElement = appenderElement;
        }

        public XElement AppenderElement { get; private set; }

        public AppenderTests Type(string type)
        {
            var actual = AppenderElement.AttributeValue("type");
            if (!actual.DontIgnoreCaseEquals(type))
                throw new Exception("Appender type expected {expected} but was {actual} for appender {name}".Replace(new
                                                                                                                     {
                                                                                                                         expected = type, 
                                                                                                                         actual = actual,
                                                                                                                         name = AppenderElement.AttributeValue("name")
                                                                                                                     }));
            return this;
        }

        public AppenderTests File(string file)
        {
            var actual = AppenderElement.RequireElement("file").AttributeValue("value");
            if (!actual.IgnoreCaseEquals(file))
                throw new Exception("Appender File expected {expected} but was {actual} for appender {name}".Replace(new
                                                                                                                     {
                                                                                                                         expected = file,
                                                                                                                         actual = actual,
                                                                                                                         name = AppenderElement.AttributeValue("name")
                                                                                                                     }));
            return this;
        }
    }
}