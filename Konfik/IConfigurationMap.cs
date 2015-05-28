using System.Collections.Generic;
using System.IO;

namespace Konfik
{
    public interface IConfigurationMap
    {
        IEnumerable<IConfigurationTest> GetTests();
        string GetTransformName(DirectoryInfo solutionDirectory, Configuration configuration);
        string GetProjectName(DirectoryInfo solutionDirectory, Configuration configuration);
    }
}