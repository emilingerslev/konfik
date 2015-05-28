using System.Linq;

namespace Konfik
{
    public static class ObjectExtensions
    {
        public static bool IsIn<T>(this T obj, params T[] inList)
        {
            if (obj == null && !inList.OfType<object>().Contains(null)) return false;
            return inList.Contains(obj);
        }
    }
}