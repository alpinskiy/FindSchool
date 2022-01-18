using System.Text.Encodings.Web;
using System.Text.Json;
using FindSchool.Core.Models;
using FindSchool.Core.Models.GeoJson;

namespace FindSchool.Core.Services;

public class GeoJsonService
{
    private readonly GeocodingService _geocodingService;

    private readonly Dictionary<int, string> _ratingColorDictionary = new()
    {
        { 0, "#793d0e" },
        { 1, "#793d0e" },
        { 2, "#793d0e" },
        { 3, "#ffd21e" },
        { 4, "#56db40" },
        { 5, "#56db40" }
    };

    public GeoJsonService(GeocodingService geocodingService)
    {
        _geocodingService = geocodingService;
    }

    public async Task GenerateGeoJsonFileAsync(
        string path, IEnumerable<School> schools, CancellationToken cancellationToken)
    {
        var features = new List<Feature>();
        foreach (var school in schools)
        {
            var feature = await CreateGeoJsonFeatureAsync(school, cancellationToken);
            if (feature != null)
            {
                features.Add(feature);
            }
        }

        var featureCollection = new FeatureCollection { Features = features };
        var geoJson = JsonSerializer.Serialize(featureCollection, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        });
        await File.WriteAllTextAsync(path, geoJson, cancellationToken);
    }

    public async Task<Feature?> CreateGeoJsonFeatureAsync(School school, CancellationToken cancellationToken)
    {
        var location = await _geocodingService.GetCoordinatesAsync(school, cancellationToken);
        if (location == null)
        {
            return null;
        }

        return new Feature
        {
            Geometry = new Geometry
            {
                Coordinates = new[]
                    { location.Value.Lon, location.Value.Lat }
            },
            Properties = new Dictionary<string, string?>
            {
                { "name", school.Name },
                { "description", school.Description },
                { "iconContent", $"{school.Number}" },
                { "iconCaption", $"{school.Rating}" },
                { "marker-color", _ratingColorDictionary[(int)school.Rating] }
            }
        };
    }
}