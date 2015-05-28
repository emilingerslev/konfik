using System;
using System.Linq;

namespace Konfik
{
    public class Configuration
    {
        private readonly string file;
        private readonly string environment;

        public string Environment
        {
            get { return environment; }
        }

        public string File
        {
            get { return file; }
        }

        public Configuration(string file, string environment)
        {
            this.file = file;
            this.environment = environment;
        }

        public override bool Equals(object obj)
        {
            return obj != null && Equals((Configuration) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public bool Equals(Configuration configuration)
        {
            if (configuration.Environment != "*" && configuration.Environment == Environment) return false;

            var leftFile = File.Split('\\');
            var rightFile = configuration.File.Split('\\');

            if (leftFile.Length != rightFile.Length) return false;
            
            if(leftFile.Take(leftFile.Length-1).Except(rightFile.Take(rightFile.Length-1), StringComparer.InvariantCultureIgnoreCase).Any()) return false;

            if(leftFile.Last() != "*" && rightFile.Last() != "*" && !leftFile.Last().Equals(rightFile.Last(), StringComparison.InvariantCultureIgnoreCase)) return false;

            return true;
        }
    }
}