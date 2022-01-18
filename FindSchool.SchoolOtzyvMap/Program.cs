using FindSchool.Core.Extensions;
using FindSchool.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FindSchool.SchoolOtzyvMap;

internal class Program : BackgroundService
{
    private static readonly string FilePath = Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "schoolotzyv.geojson");

    private readonly GeoJsonService _geoJsonService;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly SchoolOtzyvService _schoolOtzyvService;

    public Program(GeoJsonService geoJsonService, IHostApplicationLifetime hostApplicationLifetime,
        SchoolOtzyvService schoolOtzyvService)
    {
        _geoJsonService = geoJsonService;
        _hostApplicationLifetime = hostApplicationLifetime;
        _schoolOtzyvService = schoolOtzyvService;
    }

    public static Task Main(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureFindSchoolCore()
            .ConfigureServices(services =>
            {
                services
                    .AddCoreServices()
                    .AddHostedService<Program>();
            })
            .RunConsoleAsync();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await GenerateGeoJsonAsync(stoppingToken);
        _hostApplicationLifetime.StopApplication();
    }

    private async Task GenerateGeoJsonAsync(CancellationToken stoppingToken)
    {
        var list = await _schoolOtzyvService.GetSchoolListAsync(stoppingToken);
        await _geoJsonService.GenerateGeoJsonFileAsync(FilePath, list.Select(item => item.ToSchool()), stoppingToken);
    }
}