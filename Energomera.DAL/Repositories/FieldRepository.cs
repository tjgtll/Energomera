using SharpKml.Dom;
using SharpKml.Engine;
using NetTopologySuite.Geometries;
using Energomera.DTO;
using Polygon = SharpKml.Dom.Polygon;

namespace Energomera.DAL.Repositories;

public class FieldRepository : IFieldRepository
{
    private const string FieldPath = "data/fields.kml";
    private const string CentroidsPath = "data/centroids.kml";

    public List<FieldDto> GetAll()
    {
        var kmlContent = File.ReadAllText(FieldPath);
        var kmlFile = KmlFile.Load(new StringReader(kmlContent));

        var placemarks = kmlFile.Root.Flatten().OfType<Placemark>();
        var centroids = LoadCentroids();
        
        return placemarks
            .Where(p => p.Geometry is Polygon)
            .Select(p => MapToDto(p, (Polygon)p.Geometry, centroids))
            .ToList();
    }

    public FieldDto? GetById(int id) => GetAll().FirstOrDefault(f => f.Id == id);

    private Dictionary<int, Coordinate> LoadCentroids()
    {
        var centroids = new Dictionary<int, Coordinate>();
        var kmlContent = File.ReadAllText(CentroidsPath);
        var kmlFile = KmlFile.Load(new StringReader(kmlContent));

        var placemarks = kmlFile.Root.Flatten().OfType<Placemark>();
        if (placemarks != null)
        {
            foreach (var placemark in placemarks)
            {
                var data = placemark.ExtendedData?.SchemaData?.FirstOrDefault().SimpleData;
                placemark.Id = data.FirstOrDefault(d => d.Name == "fid").Text;
                if (placemark.Geometry is SharpKml.Dom.Point point &&
                    int.TryParse(placemark.Id, out var id))
                {
                    centroids[id] = new Coordinate(point.Coordinate.Longitude, point.Coordinate.Latitude);
                }
            }
        }
        return centroids;
    }

    private FieldDto MapToDto(Placemark placemark, SharpKml.Dom.Polygon kmlPolygon, Dictionary<int, Coordinate> centroids)
    {
        var name = placemark.Name;
        var data = placemark.ExtendedData?.SchemaData?.FirstOrDefault().SimpleData;
        var sizeData = data.FirstOrDefault(d => d.Name == "size");
        var id = data.FirstOrDefault(d => d.Name == "fid").Text;
        double.TryParse(sizeData.Text, out var size);
        placemark.Id = id;
        var coordinates = kmlPolygon.OuterBoundary.LinearRing.Coordinates
            .Select(c => new[] { c.Latitude, c.Longitude })
            .ToList();

        int.TryParse(placemark.Id, out var fieldId);
        centroids.TryGetValue(fieldId, out var center);
        
        var polygon = placemark.Geometry as SharpKml.Dom.Polygon;

        var coords = polygon.OuterBoundary.LinearRing.Coordinates
            .Select(c => new[] { c.Latitude, c.Longitude })
            .ToList();

        return new FieldDto
        {
            Id = fieldId,
            Name = name,
            Size = size,
            Locations = new LocationsDto
            {
                Center = center != null ? new[] { center.X, center.Y } : null,
                Polygon = coordinates
            }
        };
    }
}
