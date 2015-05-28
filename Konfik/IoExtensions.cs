using System.IO;
using System.Linq;

namespace Konfik
{
    public static class IoExtensions
    {
        public static string GetRelativePath(this DirectoryInfo directory, string absolutePath)
        {
            return GetRelativePath(directory.FullName, absolutePath);
        }

        public static string GetRelativePath(this FileInfo directory, string absolutePath)
        {
            return GetRelativePath(directory.FullName, absolutePath);
        }

        private static string GetRelativePath(string subjectPath, string absolutePath)
        {
            if (absolutePath.Last() != '\\') absolutePath = absolutePath + "\\";
            return subjectPath.Replace(absolutePath, "");
        }
    }    
}