using FindSchool.Core.HttpClients;
using FindSchool.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FindSchool.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services
            .AddHostedService<MigrationsRunner>()
            .AddTransient<APeterburgService>()
            .AddTransient<EsirService>()
            .AddTransient<GeocodingService>()
            .AddTransient<SchoolOtzyvService>()
            .AddTransient<GeoJsonService>()
            .AddDbContext<Context>();
        services.AddHttpClient<EsirHttpClient>();
        services.AddHttpClient<APeterburgHttpClient>();
        services.AddHttpClient<GoogleHttpClient>();
        services.AddHttpClient<SchoolOtzyvHttpClient>();
        return services;
    }
}