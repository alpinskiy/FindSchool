using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FindSchool.Core.Extensions;
using FindSchool.Core.Models.APeterburg;
using HtmlAgilityPack;
using Microsoft.Net.Http.Headers;

namespace FindSchool.Core.HttpClients;

public sealed class APeterburgHttpClient
{
    private readonly HttpClient _httpClient;

    private readonly Regex _raitingRegex =
        new(@"Рейтинг: ([0-9\.]+) из ([0-9]+). Голосов: ([0-9]+)", RegexOptions.Compiled);

    public APeterburgHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://apeterburg.com/");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent,
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:95.0) Gecko/20100101 Firefox/95.0");
    }

    public async IAsyncEnumerable<APeterburgItem> GetSchoolListAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "loadmore" },
            {
                "args",
                "a:5:{s:9:\"post_type\";s:6:\"school\";s:11:\"post_status\";s:7:\"publish\";s:14:\"posts_per_page\";i:30;s:7:\"orderby\";s:14:\"modified title\";s:5:\"order\";s:4:\"DESC\";}"
            },
            { "tmplt", "" },
            { "txnm", "schools" },
            { "trmid", "0" }
        };
        for (var i = 0;; ++i)
        {
            parameters["page"] = $"{i}";
            var httpResponseMessage = await _httpClient.PostAsync(
                "wp-admin/admin-ajax.php", new FormUrlEncodedContent(parameters), cancellationToken);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                yield break;
            }

            var html = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrEmpty(html))
            {
                yield break;
            }

            var count = 0;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            foreach (var node in htmlDocument.DocumentNode.SelectNodes(
                         "//div[@class='ta-f-box ta-f-box-cat']/div[@class='ta-200']"))
            {
                var nameUrlNode = node.SelectSingleNode(".//div[@class='ta-211']/a");
                var name = nameUrlNode?.InnerText?.HtmlToPlainText();
                var url = nameUrlNode?.GetAttributeValue("href", string.Empty)?.NormalizeUrl();
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(url))
                {
                    continue;
                }

                var address = node
                    .SelectSingleNode(".//div[@class='ta-212']/div[@class='one-line ol-nowrap']/div[2]")
                    .InnerText.HtmlToPlainText();
                var raitingText = node
                    .SelectSingleNode(".//span[contains(@data-tooltip,'Рейтинг:')]")
                    .GetAttributeValue("data-tooltip", string.Empty);
                var (rating, voteCount) = ParseRaitingText(raitingText);
                var viewCount = node
                    .SelectSingleNode(".//span[@class='fa-n fa-n-16 fa-n-mr5 fa-eye-808080']")
                    .NextSibling.InnerText.HtmlToPlainText();
                var commentCount = node
                    .SelectSingleNode(".//span[@class='fa-n fa-n-16 fa-n-mr5 fa-comment-o-808080']")
                    .NextSibling.InnerText.HtmlToPlainText();
                yield return new APeterburgItem(GetSchoolId(url), name)
                {
                    Address = address,
                    ViewCount = int.Parse(viewCount),
                    CommentCount = int.Parse(commentCount),
                    Rating = rating,
                    VoteCount = voteCount
                };
                ++count;
            }

            if (count == 0)
            {
                yield break;
            }
        }
    }

    public async Task<APeterburgDetails> GetSchoolDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"school/{id}", cancellationToken);
        httpResponseMessage.EnsureSuccessStatusCode();
        var html = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var node = htmlDocument.DocumentNode;
        return new APeterburgDetails
        {
            Id = id,
            FullName = node
                .SelectSingleNode("//td[@id='fullname']")?
                .InnerText.HtmlToPlainText(),
            ShortName = node
                .SelectSingleNode("//td[@id='shortname']")?
                .InnerText.HtmlToPlainText(),
            District = node
                .SelectSingleNode("//td[@id='district']")?
                .InnerText.HtmlToPlainText(),
            Kind = node
                .SelectSingleNode("//td[@id='form']")?
                .InnerText.HtmlToPlainText(),
            Url = node
                .SelectSingleNode("//td[@id='site']/a")?
                .InnerText.NormalizeUrl(),
            Email = node
                .SelectSingleNode("//td[@id='email']/a")?
                .InnerText.NormalizeUrl(),
            Contacts = string.Join('\n', node
                .SelectNodes("//span[@itemprop='telephone']")?
                .Select(item => item.InnerText.HtmlToPlainText()) ?? Enumerable.Empty<string>()),
            Director = node
                .SelectSingleNode("//td[@id='ceo']")?
                .InnerText.HtmlToPlainText(),
            Address = node
                .SelectSingleNode("//td[@id='address']")?
                .InnerText.HtmlToPlainText(),
            SubwayNearby = node
                .SelectSingleNode("//td[@id='metro']")?
                .InnerText.HtmlToPlainText(),
            ForeignLanguages = node
                .SelectSingleNode("//td[@id='languages']")?
                .InnerText.HtmlToPlainText(),
            FreeText = node
                .SelectSingleNode("//td[@id='info']")?
                .InnerText.HtmlToPlainText(),
        };
    }

    private (decimal, int) ParseRaitingText(string raitingText)
    {
        var matches = _raitingRegex.Matches(raitingText);
        var groups = matches[0].Groups;
        var ratingMax = int.Parse(groups[2].Value);
        if (ratingMax != 5)
        {
            return default;
        }

        var rating = decimal.Parse(groups[1].Value, CultureInfo.InvariantCulture);
        var voteCount = int.Parse(groups[3].Value);
        return (rating, voteCount);
    }

    private int GetSchoolId(string url)
    {
        var idText = url.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
        return int.Parse(idText);
    }
}