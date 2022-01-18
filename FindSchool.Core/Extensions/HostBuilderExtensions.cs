using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FindSchool.Core.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureFindSchoolCore(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration(builder =>
        {
            var path = Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "findschoolsettings.json");
            builder.AddJsonFile(path);
        });
        return hostBuilder;
    }
}