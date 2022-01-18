using System.Web;
using Microsoft.Extensions.Configuration;

namespace FindSchool.Core.HttpClients;

public sealed class GoogleHttpClient
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public GoogleHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/geocode/json");
        _apiKey = configuration["GoogleApiKey"];
    }

    public async Task<string> GeocodeAddressAsync(string address, CancellationToken cancellationToken)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query.Add("address", address);
        query.Add("key", _apiKey);
        var httpResponseMessage = await _httpClient.GetAsync("?" + query, cancellationToken);
        httpResponseMessage.EnsureSuccessStatusCode();
        return await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
    }
}