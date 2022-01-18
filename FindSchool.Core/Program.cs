using FindSchool.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FindSchool.Core;

public class Program : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public Program(IHostApplicationLifetime hostApplicationLifetime)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public static Task Main(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services
                    .AddCoreServices()
                    .AddHostedService<Program>();
            })
            .RunConsoleAsync();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _hostApplicationLifetime.StopApplication();
        return Task.CompletedTask;
    }
}