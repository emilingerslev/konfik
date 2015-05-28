namespace Konfik
{
    public class DirectoryMap
    {
        private readonly ProjectConfigurationMap projectConfigurationMap;
        private readonly Configuration configuration;

        public DirectoryMap(ProjectConfigurationMap projectConfigurationMap, Configuration configuration)
        {
            this.projectConfigurationMap = projectConfigurationMap;
            this.configuration = configuration;
        }

        public DirectoryMap ProjectName(string projectName)
        {
            projectConfigurationMap.ProjectName(configuration, projectName);
            return this;
        }

        public DirectoryMap TransformName(string transformName)
        {
            projectConfigurationMap.TransformName(configuration, transformName);
            return this;
        }
    }
}