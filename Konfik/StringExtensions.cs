using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Konfik
{
    public static class StringExtensions
    {
        public static bool IgnoreCaseEquals(this string left, string right)
        {
            return string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool DontIgnoreCaseEquals(this string left, string right)
        {
            return string.Equals(left, right, StringComparison.InvariantCulture);
        }

        public static string Replace(this string str, object obj)
        {
            return obj.ToDictionary().Aggregate(str, (current, pair) => current.Replace("{" + pair.Key + "}", pair.Value.ToString()));
        }

        private static IDictionary<string, object> ToDictionary(this object data)
        {
            return data.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.CanRead)
                .ToDictionary(
                    property => property.Name, 
                    property => property.GetValue(data, null));
        }
    }
}