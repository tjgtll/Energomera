using Energomera.DTO;

namespace Energomera.BLL.Services;

public interface IFieldService
{
    IEnumerable<FieldDto> GetAll();
    double? GetSize(int id);
    double? CalculateDistance(int fieldId, double lat, double lng);
    FieldBasicDto? FindFieldContainingPoint(double lat, double lng);
}
