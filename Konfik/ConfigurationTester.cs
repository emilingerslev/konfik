using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Web.XmlTransform;

namespace Konfik
{
    public class ConfigurationTester
    {
        private DirectoryInfo solutionDirectory;
        private readonly List<IConfigurationMap> maps = new List<IConfigurationMap>();
        private List<string> environments;
        private IReporter reporter = new DefaultReporter();
        private IConfigurationFinder configurationFinder = new DefaultConfigurationFinder();

        public ConfigurationTester SolutionDirectory(string path)
        {
            solutionDirectory = new DirectoryInfo(Path.GetFullPath(path));
            return this;
        }

        public ConfigurationTester ConfigurationFinder(IConfigurationFinder configurationFinder)
        {
            this.configurationFinder = configurationFinder;
            return this;
        }

        public ConfigurationTester Reporter(IReporter reporter)
        {
            this.reporter = reporter;
            return this;
        }

        public ConfigurationTester Environments(params string[] environments)
        {
            return Environments((IEnumerable<string>)environments);
        }

        public ConfigurationTester Environments(IEnumerable<string> environments)
        {
            this.environments = environments.ToList();
            return this;
        }

        public ConfigurationTester Maps(Assembly assembly)
        {
            maps.AddRange(assembly.ExportedTypes
                .Where(x => x.GetInterfaces().Contains(typeof(IProjectConfigurationMap)) && !x.IsAbstract)
                //.Where(x => x.Namespace != typeof(IProjectConfigurationMap).Namespace)
                .Select(x => (IProjectConfigurationMap)Activator.CreateInstance(x)));
            return this;
        }

        public ConfigurationTester DefaultMap<T>() where T : DefaultConfigurationMap
        {
            var configurationMap = (IConfigurationMap) Activator.CreateInstance(typeof(T));
            maps.Add(configurationMap);
            return this;
        }

        public void Test()
        {
            if(environments == null || !environments.Any())
                throw new Exception("No environments set. Use .Environments(environments...) on configuration.");

            reporter.RunningConfigurations();
            var configs = configurationFinder.FindConfigurations(solutionDirectory);
            var totalResult = configs.Aggregate(true, (totalResultAggregation, config) =>
            {
                reporter.RunningConfigurationSet(config);
                var configResult = environments.Aggregate(true, (configResultAggreation, environment) =>
                {
                    var configuration = new Configuration(config.GetRelativePath(solutionDirectory.FullName), environment);
                    reporter.RunningConfiguration(configuration);
                    var map = FindMatch(maps, configuration);
                    var testResult = RunTests(map, configuration);
                    if (testResult) reporter.PassConfiguration(configuration);
                    else reporter.FailConfiguration(configuration);
                    return testResult && configResultAggreation;
                });
                if (configResult) reporter.PassConfigurationSet(config);
                else reporter.FailConfigurationSet(config);

                return configResult && totalResultAggregation;
            });
            if (totalResult) reporter.Pass();
            else reporter.Fail();
        }

        protected IConfigurationMap FindMatch(IEnumerable<IConfigurationMap> configurationMaps,
            Configuration configuration)
        {
            var matchingMaps = configurationMaps.OfType<IProjectConfigurationMap>()
                .Where(x => x.AppliesTo.Any(a => a.Equals(configuration)))
                .ToList();
            var defualtMaps = configurationMaps.OfType<DefaultConfigurationMap>().ToList();
            if (matchingMaps.Count == 1)
                return matchingMaps.Single();
            else if (matchingMaps.Count > 1)
                throw new Exception(string.Format("Found {0} maps for \"{1}\", but 1 and only 1 map is allowed!", matchingMaps.Count, configuration.File));
            else if (defualtMaps.Count != 1)
                throw new Exception(string.Format("Found 0 maps for \"{0}\", and found {1} default maps, but 1 and only 1 default map is allowed!", configuration.File, defualtMaps.Count));
            return defualtMaps.Single();
        }

        protected bool RunTests(IConfigurationMap map, Configuration configuration)
        {
            var configurationTests = map.GetTests();
            reporter.RunningTests(configuration, configurationTests);
            return configurationTests.Aggregate(true, (result, configurationTest) =>
            {
                var context = CreateContext(map, configuration);
                try
                {
                    configurationTest.RunTest(context);
                    reporter.PassTest(configurationTest, context.TransformFile, configuration);
                }
                catch(Exception exception)
                {
                    reporter.FailTest(configurationTest, context.TransformFile, configuration, exception);
                    return false;
                }
                return result;
            });
        }

        protected ConfigurationContext CreateContext(IConfigurationMap map, Configuration configuration)
        {
            var context = new ConfigurationContext
            {
                SolutionDirectoryPath = solutionDirectory.FullName,
                Environment = configuration.Environment,
                SourceFile = configuration.File,
                ProjectName = map.GetProjectName(solutionDirectory,configuration),
            };

            var transformName = map.GetTransformName(solutionDirectory, configuration);

            context.TransformFile = Path.Combine(
                solutionDirectory.FullName, 
                Path.GetDirectoryName(context.SourceFile), 
                string.Format("{0}.{1}.config", transformName, configuration.Environment));
            var transformXmlContent = File.ReadAllText(context.TransformFile, Encoding.UTF8);

            var resultXml = new XmlDocument();
            resultXml.Load(Path.Combine(solutionDirectory.FullName, configuration.File));
            context.Source = XDocument.Parse(resultXml.OuterXml);

            var transformation = new XmlTransformation(transformXmlContent, false, null);
            transformation.Apply(resultXml);

            context.Result = XDocument.Parse(resultXml.OuterXml);

            return context;
        }
    }
}