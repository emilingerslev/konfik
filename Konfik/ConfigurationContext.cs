using System.Xml.Linq;

namespace Konfik
{
    public class ConfigurationContext
    {
        public XDocument Result { get; set; }
        public XDocument Source { get; set; }
        public string SolutionDirectoryPath { get; set; }
        public string Environment { get; set; }
        public string SourceFile { get; set; }
        public string TransformFile { get; set; }
        public string ProjectName { get; set; }
    }
}