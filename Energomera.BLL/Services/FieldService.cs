using NetTopologySuite.Geometries;
using Energomera.DAL.Repositories;
using Energomera.DTO;

namespace Energomera.BLL.Services;

public class FieldService : IFieldService
{
    private readonly IFieldRepository _repository;
    private readonly GeometryFactory _geometryFactory;

    public FieldService(IFieldRepository repository, GeometryFactory geometryFactory)
    {
        _repository = repository;
        _geometryFactory = geometryFactory; 
    }

    public IEnumerable<FieldDto> GetAll() =>  _repository.GetAll();

    public double? GetSize(int id) => _repository.GetById(id)?.Size;

    public double? CalculateDistance(int fieldId, double lat, double lng)
    {
        var field = _repository.GetById(fieldId);

        return field == null 
               ? null 
               : CalculateDistance(field.Locations.Center[0],
                                   field.Locations.Center[1],
                                   lat, 
                                   lng);
    }

    public FieldBasicDto? FindFieldContainingPoint(double lat, double lng)
    {
        var point = new Point(lat, lng);
        foreach (var field in _repository.GetAll())
        {
            if (field.Locations.Polygon != null &&
                CreatePolygon(field.Locations.Polygon).Covers(point))
            {
                return new FieldBasicDto {Id = field.Id, Name = field.Name};
            }
        }
        return null;
    }

    private Polygon CreatePolygon(List<double[]> coordinates)
    {
        var poly = coordinates.Select(c => new Coordinate(c[1], c[0])) 
                              .Append(new Coordinate(coordinates[0][1], coordinates[0][0])) 
                              .ToArray();

        return _geometryFactory.CreatePolygon(poly);
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var dLat = ToRadians(lat2 - lat1) / 2;
        var dLon = ToRadians(lon2 - lon1) / 2;
        var a = Math.Pow(Math.Sin(dLat), 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Pow(Math.Sin(dLon), 2);

        return 6371000 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private double ToRadians(double angle) => Math.PI * angle / 180.0;
}
