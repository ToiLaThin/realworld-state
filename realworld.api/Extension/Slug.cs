using System.Text.RegularExpressions;

namespace Realworld.Api.Extension
{
    public static class SlugGenerator {
        public static string GenerateSlug(this string phrase) {
            string str = phrase.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // anything beside these character are invalid chars and will be replaced with empty string
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim
            str = Regex.Replace(str, @"\s", "-"); // hyphens
            return str;
        }
    }
}