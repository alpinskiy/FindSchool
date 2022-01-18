using FindSchool.Core.HttpClients;
using FindSchool.Core.Models.APeterburg;
using Microsoft.EntityFrameworkCore;

namespace FindSchool.Core.Services;

public class APeterburgService
{
    private readonly Context _context;
    private readonly APeterburgHttpClient _httpClient;

    public APeterburgService(Context context, APeterburgHttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<List<APeterburgInfo>> GetSchoolListAsync(CancellationToken cancellationToken)
    {
        await EnsureDataLoaded(cancellationToken);
        var shallowInfoList = await _context.ApSchools.ToListAsync(cancellationToken);
        var list = new List<APeterburgInfo>(shallowInfoList.Count);
        foreach (var shallowInfo in shallowInfoList)
        {
            var details = await _context.ApSchoolDetails.FindAsync(new object[] { shallowInfo.Id }, cancellationToken);
            var apSchool = new APeterburgInfo(shallowInfo, details);
            list.Add(apSchool);
        }

        return list;
    }

    private async Task EnsureDataLoaded(CancellationToken cancellationToken)
    {
        if (await _context.ApSchools.CountAsync(cancellationToken) == 0)
        {
            await LoadStatisticsAsync(cancellationToken);
        }

        if (await _context.ApSchoolDetails.CountAsync(cancellationToken) == 0)
        {
            await LoadDetailsAsync(cancellationToken);
        }
    }

    private async Task LoadStatisticsAsync(CancellationToken cancellationToken)
    {
        var knownIds = new HashSet<int>(await _context.ApSchools
            .Select(item => item.Id).ToListAsync(cancellationToken));
        await foreach (var entity in _httpClient.GetSchoolListAsync(cancellationToken))
        {
            if (!knownIds.Contains(entity.Id))
            {
                await _context.ApSchools.AddAsync(entity, cancellationToken);
                knownIds.Add(entity.Id);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task LoadDetailsAsync(CancellationToken cancellationToken)
    {
        var knownSchoolIds = new HashSet<int>(
            await _context.ApSchools.Select(item => item.Id).ToListAsync(cancellationToken));
        var knownDetailIds = new HashSet<int>(
            await _context.ApSchoolDetails.Select(item => item.Id).ToListAsync(cancellationToken));
        foreach (var id in knownSchoolIds.Where(id => !knownDetailIds.Contains(id)))
        {
            var details = await _httpClient.GetSchoolDetailsAsync(id, cancellationToken);
            await _context.ApSchoolDetails.AddAsync(details, cancellationToken);
            knownDetailIds.Add(id);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}