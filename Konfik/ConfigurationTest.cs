using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Konfik
{
    public abstract class ConfigurationTest : IConfigurationTest
    {
        public void RunTest(ConfigurationContext context)
        {
            Context = context;
            Result = context.Result;
            ProjectName = context.ProjectName;
            Environment = context.Environment;
            Test();
        }

        protected ConfigurationContext Context { get; private set; }
        protected XDocument Result { get; private set; }
        protected string ProjectName { get; private set; }
        protected string Environment { get; private set; }
        protected abstract void Test();

        protected IEnumerable<XElement> AppSettings()
        {
            return Result.Root.Elements("appSettings")
                .ThrowIfCountIsnt(1, "Expected 1 and only 1 <appSettings>, but found {count}")
                .Single()
                .Elements("add");
        }

        protected XElement AppSetting(string key, string value)
        {
            return AppSettings()
                .Where(x => x.AttributeValue("key") == key)
                .ThrowIfCountIsnt(1, "Expected 1 and only 1 appSetting with key=\"{key}\", but found {count}".Replace(new {key}))
                .Single()
                .ThrowIf(x => x.AttributeValue("value") != value, x => "Expected appSetting key=\"{key}\" had value=\"{expected}\" but actual value=\"{actual}\""
                    .Replace(new {key, expected = value, actual = x.AttributeValue("value") }));
        }

        protected IEnumerable<XElement> ConnectionStrings()
        {
            return Result.Root.Elements("connectionStrings")
                .ThrowIfCountIsnt(1, "Expected 1 and only 1 <connectionStrings>, but found {count}")
                .Single()
                .Elements("add");
        }

        protected XElement ConnectionString(string name, string connectionString)
        {
            return ConnectionStrings()
                .Where(x => x.AttributeValue("name") == name)
                .ThrowIfCountIsnt(1, "Expected 1 and only 1 connectionString with name=\"{name}\", but found {count}".Replace(new {name}))
                .Single()
                .ThrowIf(x => x.AttributeValue("connectionString") != connectionString, 
                    x => "Expected connectionString name=\"{name}\" had connectionString=\"{expected}\" but actual connectionString=\"{actual}\""
                        .Replace(new {name, expected = connectionString, actual = x.AttributeValue("connectionString") }));
        }
    }
}