using System.Collections.Generic;
using System.IO;

namespace Konfik
{
    public interface IConfigurationFinder
    {
        IEnumerable<FileInfo> FindConfigurations(DirectoryInfo solutionDirectory);
    }
}