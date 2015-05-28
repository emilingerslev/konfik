using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Konfik
{
    public class DefaultReporter : IReporter
    {
        public void RunningConfigurations()
        {
            
        }

        public void RunningConfigurationSet(FileInfo config)
        {
            Trace.Write("{config}\n".Replace(new {config}));
        }

        public void RunningConfiguration(Configuration configuration)
        {
            Trace.Write("{Environment}: ".Replace(new {configuration.Environment}));
        }

        public void RunningTests(Configuration configuration, IEnumerable<IConfigurationTest> configurationTests)
        {
        }


        public void PassTest(IConfigurationTest configurationTest, string transformFile, Configuration configuration)
        {
            Trace.Write(".");
        }

        public void FailTest(IConfigurationTest configurationTest, string transformFile, Configuration configuration, Exception exception)
        {
            Trace.Write("\nFail {testName}:\n{exception}\n".Replace(new {exception, testName = configurationTest.GetType().Name}));
        }


        public void PassConfiguration(Configuration configuration)
        {
            Trace.Write("passed\n");
        }

        public void FailConfiguration(Configuration configuration)
        {
            
        }


        public void PassConfigurationSet(FileInfo config)
        {
            Trace.Write("All passed\n\n");
        }

        public void FailConfigurationSet(FileInfo config)
        {
            Trace.Write("Tests failed\n\n");
            //Trace.WriteLine("");
        }


        public void Pass()
        {
            Trace.Write("\n");
            Trace.Write("--------------------------------------------------------\n");
            Trace.Write("All configurations in all environments passed all tests.\n");
            Trace.Write("--------------------------------------------------------\n");
        }

        public void Fail()
        {
            Trace.Write("\n");
            Trace.Write("--------------------------------------------------------\n");
            Trace.Write("Tests failed!.\n");
            Trace.Write("--------------------------------------------------------\n");

            throw new Exception("Configuration tests failed.");
        }
    }
}