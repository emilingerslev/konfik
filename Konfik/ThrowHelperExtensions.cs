using System;
using System.Collections.Generic;
using System.Linq;

namespace Konfik
{
    public static class ThrowHelperExtensions
    {
        public static T ThrowIf<T>(this T obj, Func<T, bool> throwCondition, string message)
        {
            if (throwCondition(obj))
                throw new Exception(message);
            return obj;
        }

        public static T ThrowIf<T>(this T obj, Func<T, bool> throwCondition, Func<T, string> message)
        {
            if (throwCondition(obj))
                throw new Exception(message(obj));
            return obj;
        }
        
        public static T ThrowIfNull<T>(this T obj, string message)
        {
            return obj.ThrowIf(arg => arg == null, message);
        }

        public static IEnumerable<T> ThrowIfCountIsnt<T>(this IEnumerable<T> enumerable, int count, string message)
        {
            var actualCount = enumerable.Count();
            return enumerable.ThrowIf(arg => actualCount != count, message.Replace(new { count = actualCount }));
        }

        
    }
}