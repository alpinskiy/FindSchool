using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using FindSchool.Core.HttpClients;
using FindSchool.Core.Models;
using FindSchool.Core.Models.Google;
using Microsoft.EntityFrameworkCore;

namespace FindSchool.Core.Services;

public class GeocodingService
{
    private readonly Context _context;
    private readonly GoogleHttpClient _httpClient;

    public GeocodingService(Context context, GoogleHttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<LatLng?> GetCoordinatesAsync(School school, CancellationToken cancellationToken)
    {
        var address = school.Address;
        if (string.IsNullOrWhiteSpace(address))
        {
            address = school.Name;
        }

        var geocoding = await _context.Geocodings
            .Where(item => item.Address == address ||
                           item.SchoolOtzyvId != null && item.SchoolOtzyvId == school.SchoolOtzyvId ||
                           item.APeterburgId != null && item.APeterburgId == school.APeterburgId)
            .FirstOrDefaultAsync(cancellationToken);
        if (geocoding == null)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentOutOfRangeException(nameof(school), school, "School address is empty");
            }

            var response = await _httpClient.GeocodeAddressAsync(address, cancellationToken);
            if (geocoding != null)
            {
                geocoding.Response = response;
            }
            else
            {
                geocoding = new Geocoding(address, response)
                {
                    APeterburgId = school.APeterburgId,
                    SchoolOtzyvId = school.SchoolOtzyvId
                };
                await _context.Geocodings.AddAsync(geocoding, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        return TryGetCoordinates(geocoding, out var coordinates)
            ? coordinates
            : null;
    }

    private bool TryGetCoordinates(Geocoding geocoding, [NotNullWhen(true)] out LatLng? coordinates)
    {
        var response = JsonSerializer.Deserialize<GeocodingResponse>(geocoding.Response);
        coordinates = response.Results?.FirstOrDefault()?.Geometry.Location;
        return coordinates != null;
    }
}