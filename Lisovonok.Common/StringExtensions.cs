using System.Collections.Generic;
using System.Linq;

namespace Lisovonok.Common
{
    public static class StringExtensions
    {
        public static string Trim(this string value, int maxlength)
        {
            if (string.IsNullOrEmpty(value) || maxlength > value.Length)
            {
                return value;
            }
            return value.Substring(0, maxlength - 4) + " ...";
        }

        public static string GetTextOrNullIfEmpty(this string value)
        {
            if (value == null || value.Trim().Length == 0)
            {
                return null;
            }
            return value;
        }

        public static string ToString<T>(this IEnumerable<T> values, string separator)
        {
            if (values == null)
            {
                return string.Empty;
            }
            var valueArray = values.ToArray();
            return string.Join(separator, values.Select(v => v.ToString()).ToArray());
        }
    }
}
