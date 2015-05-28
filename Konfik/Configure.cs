namespace Konfik
{
    public static class Configure
    {
        public static ConfigurationTester WithSolutionDirectory(string solutionDirectory)
        {
            return new ConfigurationTester().SolutionDirectory(solutionDirectory);
        }
    }
}