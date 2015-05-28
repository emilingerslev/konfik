using System.Collections.Generic;

namespace Konfik
{
    public abstract class ProjectConfigurationMap : ConfigurationMap, IProjectConfigurationMap
    {
        public DirectoryMap Directory(string directoryPath, string environment = "*")
        {
            var configuration = new Configuration(directoryPath + "\\*", environment);
            appliesTo.Add(configuration);
            return new DirectoryMap(this, configuration);
        }

        private IList<Configuration> appliesTo = new List<Configuration>();
        public IEnumerable<Configuration> AppliesTo { get { return appliesTo; } }
    }
}