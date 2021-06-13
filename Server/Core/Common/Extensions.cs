using Connect.LanguagePackManager.Core.Models.Texts;
using System;
using System.Text.RegularExpressions;

namespace Connect.LanguagePackManager.Core.Common
{
    public static class Extensions
    {
        public static Version ParseVersion(this string tagName)
        {
            var m = Regex.Match(tagName, @"v?(\d+)\.(\d+)\.(\d+)");
            if (m.Success)
            {
                return new Version(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
            }
            return new Version(0, 0, 0);
        }

        public static string ToNormalizedFormat(this Version input)
        {
            return $"{input.Major.ToString("00")}.{input.Minor.ToString("00")}.{input.Build.ToString("00")}";
        }

        public static string EnsureEndsWith(this string input, string ending)
        {
            if (!input.EndsWith(ending))
            {
                input += ending;
            }
            return input;
        }

        public static bool IsSmallerThan(this string input, string compareString)
        {
            return input.CompareTo(compareString) < 0;
        }
        public static bool IsBiggerThan(this string input, string compareString)
        {
            return input.CompareTo(compareString) > 0;
        }

        public static bool CoversVersion(this Text input, string normalizedVersion)
        {
            var res = !input.Version.IsBiggerThan(normalizedVersion);
            if (res && !string.IsNullOrEmpty(input.DeprecatedInVersion))
            {
                res = input.DeprecatedInVersion.IsBiggerThan(normalizedVersion);
            }

            return res;
        }
    }
}
