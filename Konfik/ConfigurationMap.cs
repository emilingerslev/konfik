using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Konfik
{
    public abstract class ConfigurationMap : IConfigurationMap
    {
        private IList<IConfigurationTest> tests = new List<IConfigurationTest>();
        private readonly IDictionary<Configuration, string> projectNames = new Dictionary<Configuration, string>();
        private readonly IDictionary<Configuration, string> transformNames = new Dictionary<Configuration, string>();

        public ConfigurationMap()
        {
            Map();
        }

        public string GetTransformName(DirectoryInfo solutionDirectory, Configuration configuration)
        {
            if (transformNames.ContainsKey(configuration)) return transformNames[configuration];
            var transformName = Path.GetFileNameWithoutExtension(configuration.File);
            if (transformName.Equals("app", StringComparison.InvariantCultureIgnoreCase))
            {
                transformName = GetProjectName(solutionDirectory, configuration) + ".exe";
            }
            return transformName;
        }

        public string GetProjectName(DirectoryInfo solutionDirectory, Configuration configuration)
        {
            if(projectNames.ContainsKey(configuration)) return projectNames[configuration];
            var fullPath = Path.Combine(solutionDirectory.FullName, Path.GetDirectoryName(configuration.File));
            var projectFile = Directory.GetFiles(fullPath, "*.csproj").SingleOrDefault();
            if (projectFile != null) return Path.GetFileNameWithoutExtension(projectFile);
            return new DirectoryInfo(Path.GetDirectoryName(configuration.File)).Name;
        }

        public IEnumerable<IConfigurationTest> GetTests()
        {
            return tests;
        }

        protected abstract void Map();

        protected T Test<T>() where T : IConfigurationTest, new()
        {
            tests.Add((IConfigurationTest)Activator.CreateInstance(typeof(T)));
            return default(T);
        }

        protected IConfigurationTest Test(IConfigurationTest configurationTest)
        {
            tests.Add(configurationTest);
            return configurationTest;
        }

        public void ProjectName(Configuration configuration, string projectName)
        {
            projectNames[configuration] = projectName;
        }

        public void TransformName(Configuration configuration, string transformName)
        {
            transformNames[configuration] = transformName;
        }
    }
}