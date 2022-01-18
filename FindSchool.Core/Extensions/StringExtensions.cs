using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace FindSchool.Core.Extensions;

public static class StringExtensions
{
    private static readonly Regex NumberRegex = new("[0-9]+", RegexOptions.Compiled);

    public static string HtmlToPlainText(this string text)
    {
        return HtmlEntity.DeEntitize(text).Trim();
    }

    public static string NormalizeUrl(this string text)
    {
        return new UriBuilder(text).Uri.ToString();
    }

    public static bool TryGetSingleNumber(this string? text, out int number)
    {
        if (string.IsNullOrEmpty(text))
        {
            number = 0;
            return false;
        }

        var matches = NumberRegex.Matches(text);
        if (matches.Count == 0)
        {
            number = 0;
            return false;
        }

        number = int.Parse(matches[0].Value);
        return matches.Skip(1).All(item =>
            string.Equals(item.Value, matches[0].Value, StringComparison.Ordinal));
    }
}