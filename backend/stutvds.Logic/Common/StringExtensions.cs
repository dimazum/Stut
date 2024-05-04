using System;
using System.Text.RegularExpressions;

namespace stutvds.Logic.Common
{
    public static class StringExtensions
    {
        public static string CleanBraketsWithContent(this string value)
        {
            return Regex.Replace(value, @" ?\(.*?\)", string.Empty);
        }

        public static Guid? ToGuid(this string? value)
        {
            if(value != null)
            {
                return new Guid(value);
            }
            return null;
        }
    }
}