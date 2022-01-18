using FindSchool.Core.HttpClients;
using FindSchool.Core.Models.SchoolOtzyv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FindSchool.Core.Services;

public class SchoolOtzyvService
{
    private readonly Context _context;
    private readonly ILogger<SchoolOtzyvService> _logger;
    private readonly SchoolOtzyvHttpClient _schoolOtzyvHttpClient;

    public SchoolOtzyvService(Context context, ILogger<SchoolOtzyvService> logger,
        SchoolOtzyvHttpClient schoolOtzyvHttpClient)
    {
        _context = context;
        _logger = logger;
        _schoolOtzyvHttpClient = schoolOtzyvHttpClient;
    }

    public async Task<List<SchoolOtzyvInfo>> GetSchoolListAsync(CancellationToken cancellationToken)
    {
        await EnsureSchoolOtzyvItemsLoaded(cancellationToken);
        // TODO:
        //await EnsureSchoolOtzyvDetailsLoaded(cancellationToken);
        var list = new List<SchoolOtzyvInfo>();
        var schoolOtzyvItems = await _context.SchoolOtzyvItems.ToListAsync(cancellationToken);
        foreach (var item in schoolOtzyvItems)
        {
            var details = await _context.SchoolOtzyvDetails.FindAsync(new object[] { item.Id }, cancellationToken);
            list.Add(new SchoolOtzyvInfo(item, details));
        }

        return list;
    }

    private async Task EnsureSchoolOtzyvItemsLoaded(CancellationToken cancellationToken)
    {
        if (0 < await _context.SchoolOtzyvItems.CountAsync(cancellationToken))
        {
            return;
        }

        var schoolOtzyvItems = await _schoolOtzyvHttpClient.GetSchoolListAsync(cancellationToken);
        await _context.SchoolOtzyvItems.AddRangeAsync(schoolOtzyvItems, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureSchoolOtzyvDetailsLoaded(CancellationToken cancellationToken)
    {
        if (0 < await _context.SchoolOtzyvDetails.CountAsync(cancellationToken))
        {
            return;
        }

        var schoolOtzyvItems = await _context.SchoolOtzyvItems.ToListAsync(cancellationToken);
        foreach (var item in schoolOtzyvItems)
        {
            var details = await _context.SchoolOtzyvDetails.FindAsync(new object[] { item.Id }, cancellationToken);
            if (details != null)
            {
                continue;
            }

            for (var i = 1;; i++)
            {
                try
                {
                    details = await _schoolOtzyvHttpClient.GetSchoolDetailsAsync(item, cancellationToken);
                    break;
                }
                catch (HttpRequestException exception) when (i < 10)
                {
                    _logger.LogInformation(exception, string.Empty);
                    await Task.Delay(TimeSpan.FromSeconds(5 * i), cancellationToken);
                }
            }

            await _context.SchoolOtzyvDetails.AddAsync(details, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}