using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace AutoClass
{
    public static class Utils
    {
        public static string Combine(string separator, params string[] strings)
        {
            StringBuilder sb = new();

            for (int i = 0; i < strings.Length; i++)
            {
                if (i > 0)
                    sb.Append(separator);
                sb.Append(strings[i]);
            }

            return sb.ToString();
        }

        public static IEnumerable<TEnum> GetEnumValues<TEnum>() where TEnum : struct, Enum
        {
            return from TEnum value in Enum.GetValues(typeof(TEnum)) select value;
        }

        public static string Repeat(string text, int count)
        {
            string r = "";
            for (int i = 0; i < count; i++)
                r += text;
            return r;
        }

        public static IEnumerable<T> Repeat<T>(T value, int count)
        {
            for (int i = 0; i < count; i++)
                yield return value;
        }

        public static string GetBaseTypes(string types)
        {
            return string.IsNullOrEmpty(types)
                ? string.Empty
                : ": " + Combine(", ", types);
        }

        public static string AsReturnType(this string type, bool addSpace = true) => type switch
        {
            _ when string.IsNullOrWhiteSpace(type) => "void",
            _ => type
        } + (addSpace ? " " : string.Empty);

        public static TEnum[] GetFlags<TEnum>(this TEnum @enum) where TEnum : struct, Enum
        {
            return (from TEnum flag in Enum.GetValues(typeof(TEnum))
                    where @enum.HasFlag(flag)
                    select flag) as TEnum[];
        }
    }
}
