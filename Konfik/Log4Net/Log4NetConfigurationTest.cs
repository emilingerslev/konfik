using System;
using System.Linq;
using System.Xml.Linq;

namespace Konfik.Log4Net
{
    public abstract class Log4NetConfigurationTest : ConfigurationTest
    {
        public LoggerTests Logger(string loggerName)
        {
            var loggerElements = Section().Elements("logger", x => x.AttributeValue("name").IgnoreCaseEquals(loggerName)).ToList();
            if (loggerElements.Count != 1) throw new Exception("Found {Count} loggers named {loggerName} but expected 1 and only 1".Replace(new {loggerElements.Count, loggerName}));
            return new LoggerTests(this, loggerElements.Single());
        }

        public AppenderTests Appender(string appenderName)
        {
            var appenderElements = Section().Elements("appender", x => x.AttributeValue("name").IgnoreCaseEquals(appenderName)).ToList();
            if (appenderElements.Count != 1) throw new Exception("Found {Count} loggers named {loggerName} but expected 1 and only 1".Replace(new { appenderElements.Count, appenderName }));
            return new AppenderTests(this, appenderElements.Single());
        }

        public XElement Section()
        {
            return Result.ConfigSectionByType("log4net.Config.Log4NetConfigurationSectionHandler, log4net");
        }
    }
}