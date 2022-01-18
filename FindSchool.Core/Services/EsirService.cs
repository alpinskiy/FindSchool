using FindSchool.Core.HttpClients;
using Microsoft.EntityFrameworkCore;

namespace FindSchool.Core.Services;

public class EsirService
{
    private readonly Context _context;
    private readonly EsirHttpClient _httpClient;

    public EsirService(Context context, EsirHttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task LoadEsirSchoolListAsync(CancellationToken cancellationToken)
    {
        var schools = await _context.EsirSchools
            .Where(item => !string.IsNullOrEmpty(item.Url))
            .ToDictionaryAsync(item => item.Url, cancellationToken);
        await foreach (var entity in _httpClient.GetSchoolListAsync(cancellationToken))
        {
            if (!schools.TryGetValue(entity.Url, out var school))
            {
                var entry = await _context.EsirSchools.AddAsync(entity, cancellationToken);
                school = entry.Entity;
                schools.Add(entity.Url, school);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}