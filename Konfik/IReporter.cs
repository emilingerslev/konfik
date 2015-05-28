using System;
using System.Collections.Generic;
using System.IO;

namespace Konfik
{
    public interface IReporter
    {
        void RunningConfigurations();

        void RunningTests(Configuration configuration, IEnumerable<IConfigurationTest> configurationTests);
        void RunningConfigurationSet(FileInfo config);
        void RunningConfiguration(Configuration configuration);

        void PassTest(IConfigurationTest configurationTest, string transformFile, Configuration configuration);
        void FailTest(IConfigurationTest configurationTest, string transformFile, Configuration configuration, Exception exception);

        void PassConfiguration(Configuration configuration);
        void FailConfiguration(Configuration configuration);

        void PassConfigurationSet(FileInfo config);
        void FailConfigurationSet(FileInfo config);

        void Pass();
        void Fail();
    }
}