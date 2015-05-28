using System;

namespace Konfik
{
    public class ConfigurationTestException : Exception
    {
        public string TransformFile { get; private set; }
        public Configuration Configuration { get; private set; }

        public ConfigurationTestException(string transformFile, Configuration configuration, Exception exception) :
            base("Test failed for {transformFile}: {Message}".Replace(new { transformFile, exception.Message}), exception)
        {
            TransformFile = transformFile;
            Configuration = configuration;
        }
    }
}