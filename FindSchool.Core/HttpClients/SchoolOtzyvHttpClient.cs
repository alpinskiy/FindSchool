using System.Globalization;
using FindSchool.Core.Extensions;
using FindSchool.Core.Models.SchoolOtzyv;
using HtmlAgilityPack;
using Microsoft.Net.Http.Headers;

namespace FindSchool.Core.HttpClients;

public class SchoolOtzyvHttpClient
{
    private readonly HttpClient _httpClient;

    public SchoolOtzyvHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://schoolotzyv.ru/");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent,
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:95.0) Gecko/20100101 Firefox/95.0");
    }

    public async Task<List<SchoolOtzyvItem>> GetSchoolListAsync(CancellationToken cancellationToken)
    {
        var httpResponseMessage = await _httpClient.GetAsync(
            "schools/9-russia/173-sankt-peterburg", cancellationToken);
        httpResponseMessage.EnsureSuccessStatusCode();
        var html = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        var list = new List<SchoolOtzyvItem>();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        foreach (var node in htmlDocument.DocumentNode.SelectNodes(
                     "//tr[@class='sectiontableentry']"))
        {
            var id = node
                .SelectSingleNode(".//img[@class='preview-list lazy']")
                .GetAttributeValue("data-src", string.Empty)
                .Split('/', StringSplitOptions.RemoveEmptyEntries)[2];
            var url = node.SelectSingleNode(".//a")?.GetAttributeValue("href", string.Empty);
            var name = node.SelectSingleNode(".//a")?.InnerText.HtmlToPlainText();
            var rating = node.SelectSingleNode(".//td[@data-label='Рейтинг по отзывам']")?.InnerText;
            var commentCount = node.SelectSingleNode(".//td[@data-label='Отзывов о школе']")?.InnerText;
            list.Add(new SchoolOtzyvItem
            {
                Id = int.Parse(id),
                Name = name,
                Url = url,
                Rating = decimal.Parse(rating!, CultureInfo.InvariantCulture),
                CommentCount = int.Parse(commentCount!)
            });
        }

        return list;
    }

    public async Task<SchoolOtzyvDetails> GetSchoolDetailsAsync(
        SchoolOtzyvItem item, CancellationToken cancellationToken)
    {
        var httpResponseMessage = await _httpClient.GetAsync(item.Url, cancellationToken);
        httpResponseMessage.EnsureSuccessStatusCode();
        var html = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var address = htmlDocument.DocumentNode
            .SelectSingleNode("//span[@itemprop='address']")?
            .InnerText.HtmlToPlainText();
        return new SchoolOtzyvDetails
        {
            Address = address
        };
    }
}