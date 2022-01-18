using FindSchool.Core.Extensions;
using FindSchool.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FindSchool.APeterburgMap;

internal class Program : BackgroundService
{
    private static readonly string FilePath = Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "apeterburg.geojson");

    private readonly APeterburgService _aPeterburgService;
    private readonly GeoJsonService _geoJsonService;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public Program(
        APeterburgService aPeterburgService,
        GeoJsonService geoJsonService,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _aPeterburgService = aPeterburgService;
        _geoJsonService = geoJsonService;
        _hostApplicationLifetime = hostApplicationLifetime;
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
        var schools = await _aPeterburgService.GetSchoolListAsync(stoppingToken);
        await _geoJsonService.GenerateGeoJsonFileAsync(
            FilePath, schools.Select(item => item.ToSchool()), stoppingToken);
    }
}