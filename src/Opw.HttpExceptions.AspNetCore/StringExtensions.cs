using Opw.HttpExceptions.AspNetCore.Serialization;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Opw.HttpExceptions.AspNetCore
{
    internal static class StringExtensions
    {
        internal static string ToCamelCase(this string s)
        {
            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        internal static string ToSlug(this string s)
        {
            string str = s.RemoveDiacritics();
            // add add a space before every upercase char
            str = Regex.Replace(str, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
            str = str.ToLower();

            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // trim 
            str = str.Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        /// <summary>
        /// Remove diacritics (accents) from a string.
        /// </summary>
        /// <remarks>See: "http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net"</remarks>
        internal static string RemoveDiacritics(this string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        internal static SerializableException ReadAsSerializableException(this string json)
        {
            var converter = new SerializableExceptionJsonConverter();
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            return converter.Read(ref reader, typeof(SerializableException), new JsonSerializerOptions());
        }
    }
}
