using Connect.LanguagePackManager.Core.Models.Texts;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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
      var res = !input.FirstInVersion.IsBiggerThan(normalizedVersion) && input.DeprecatedInVersion.IsBiggerThan(normalizedVersion);

      return res;
    }

    public static string ToMD5Hash(this string input)
    {
      var c = new MD5CryptoServiceProvider();
      return BitConverter.ToString(c.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }

    public static bool OnOffToBool(this string input)
    {
      return input.ToLowerInvariant() == "on";
    }

    public static string ReplaceEnd(this string input, string replace, string replaceWith)
    {
      if (!input.EndsWith(replace)) return input;
      input = input.Substring(0, input.Length - replace.Length);
      return input + replaceWith;
    }

    public static void WriteFileToZip(this ZipArchive input, string fileName, byte[] fileData)
    {
      var newEntry = input.CreateEntry(fileName);
      using (var zipStream = newEntry.Open())
      {
        try
        {
          zipStream.Write(fileData, 0, fileData.Length);
        }
        catch (Exception ex)
        {
        }
      }
    }

    public static string ParseRegion(this System.Globalization.CultureInfo input)
    {
      if (input.IsNeutralCulture) return input.EnglishName;
      var m = Regex.Match(input.EnglishName, @"\((.+)\)");
      if (m.Success)
      {
        var region = m.Groups[1].Value;
        if (region.IndexOf(",") > 0)
        {
          region = region.Substring(region.IndexOf(",") + 2);
        }
        return region;
      }
      return input.EnglishName;
    }

    public static string GetResponseBody(this HttpWebResponse input)
    {
      if (input == null) return "";
      try
      {
        var dataStream = input.GetResponseStream();
        var reader = new StreamReader(dataStream);
        var responseFromServer = reader.ReadToEnd();
        return responseFromServer;
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
}
