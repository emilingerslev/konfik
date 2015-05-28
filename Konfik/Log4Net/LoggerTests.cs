using System;
using System.Xml.Linq;

namespace Konfik.Log4Net
{
    public class LoggerTests
    {
        private readonly Log4NetConfigurationTest log4NetConfigurationTest;

        public LoggerTests(Log4NetConfigurationTest log4NetConfigurationTest, XElement loggerElement)
        {
            this.log4NetConfigurationTest = log4NetConfigurationTest;
            LoggerElement = loggerElement;
        }

        public XElement LoggerElement { get; private set; }

        public LoggerTests Level(string level)
        {
            LoggerElement.RequireElement("level").AttributeValue("value").DontIgnoreCaseEquals("DEBUG");
            return this;
        }

        public LoggerTests AppenderRef(string name, Action<AppenderTests> tests = null)
        {
            LoggerElement.RequireElement("appender-ref", x => x.AttributeValue("ref").DontIgnoreCaseEquals(name));
            if(tests != null)
                tests(log4NetConfigurationTest.Appender(name));
            return this;
        }
    }
}