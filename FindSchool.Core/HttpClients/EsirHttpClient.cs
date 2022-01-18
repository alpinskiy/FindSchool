using System.Runtime.CompilerServices;
using FindSchool.Core.Extensions;
using FindSchool.Core.Models.Esir;
using HtmlAgilityPack;
using Microsoft.Net.Http.Headers;

namespace FindSchool.Core.HttpClients;

public sealed class EsirHttpClient
{
    private static readonly EsirCategory[] EsirSchoolCategories =
    {
        EsirCategory.Lyceum,
        EsirCategory.SecondarySchool,
        EsirCategory.Gymnasium,
        EsirCategory.PrimarySchool,
        EsirCategory.EducationCenter
    };

    private readonly HttpClient _httpClient;

    public EsirHttpClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://esir.gov.spb.ru/");
        httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent,
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:95.0) Gecko/20100101 Firefox/95.0");
        _httpClient = httpClient;
    }

    public async IAsyncEnumerable<EsirInfo> GetSchoolListAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await _httpClient.GetAsync("set_lang/?language=ru", cancellationToken);
        foreach (var category in EsirSchoolCategories)
        {
            await foreach (var item in GetSchoolListAsync(category, cancellationToken))
            {
                yield return item;
            }
        }
    }

    private async IAsyncEnumerable<EsirInfo> GetSchoolListAsync(
        EsirCategory category, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var previousHtml = string.Empty;
        for (var page = 1;; ++page)
        {
            var uri = $"category/{(int)category}/?page={page}";
            var httpResponseMessage = await _httpClient.GetAsync(uri, cancellationToken);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                yield break;
            }

            var html = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            if (string.Equals(html, previousHtml, StringComparison.Ordinal))
            {
                yield break;
            }

            previousHtml = html;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            foreach (var node in htmlDocument.DocumentNode.SelectNodes("//ul[@class='siteList']/li/a"))
            {
                var url = node.Attributes["href"].Value.NormalizeUrl();
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }

                var title = node.SelectSingleNode("strong").InnerText.HtmlToPlainText();
                yield return new EsirInfo(category, url) { UrlText = title };
            }
        }
    }
}