using System.Collections.Generic;

namespace Konfik
{
    public interface IProjectConfigurationMap : IConfigurationMap
    {
        IEnumerable<Configuration> AppliesTo { get; }
    }
}