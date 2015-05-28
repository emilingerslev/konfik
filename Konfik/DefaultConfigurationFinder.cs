using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Konfik
{
    public class DefaultConfigurationFinder : IConfigurationFinder
    {
        private Func<FileInfo, bool> filter = file => true;

        public IEnumerable<FileInfo> FindConfigurations(DirectoryInfo solutionDirectory)
        {
            var configs = Directory.GetFiles(solutionDirectory.FullName, "*.config", SearchOption.AllDirectories)
                .Select(x => new FileInfo(x))
                .Where(x => !x.Name.Equals("web.config", StringComparison.InvariantCultureIgnoreCase) || x.Directory.GetFiles("*.csproj").Any()).ToList()
                .Where(x => x.Name.Equals("web.config", StringComparison.InvariantCultureIgnoreCase) || x.Name.Equals("app.config", StringComparison.InvariantCultureIgnoreCase))
                //.Where(x => x.Directory.Name != "bin" && x.Directory.Parent.Name != "bin")
                .Where(filter)
                .ToList();
            return configs;
        }

        public DefaultConfigurationFinder Filter(Func<FileInfo, bool> filter)
        {
            this.filter = filter;
            return this;
        }
    }
}