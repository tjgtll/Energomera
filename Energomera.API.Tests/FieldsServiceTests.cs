using Energomera.BLL.Services;
using Energomera.DAL.Repositories;
using NetTopologySuite.Geometries;

namespace Energomera.API.Tests;

public class FieldsServiceTests
{
    [Fact]
    public void GetAllTest()
    {
        // Arrange
        var fieldService = new FieldService(new FieldRepository(), new GeometryFactory());

        // Act
        var fields = fieldService.GetAll();

        // Assert
        Assert.NotEmpty(fields);
        Assert.True(fields.All(f => f.Id > 0));
        Assert.True(fields.All(f => !string.IsNullOrEmpty(f.Name)));
        Assert.True(fields.Count() == 6);
    }

    [Theory]
    [InlineData(1, 100d)]
    [InlineData(7, null)]
    public void GetSizeTest(int fieldId, double? expectedSize)
    {
        // Arrange
        var fieldService = new FieldService(new FieldRepository(), new GeometryFactory());

        // Act
        var result = fieldService.GetSize(fieldId);

        // Assert
        Assert.True(result == expectedSize);
    }

    [Theory]
    [InlineData(1, 41.3380610642585d, 45.6962567581079d, 0d)]
    [InlineData(2, 41.3380610642585d, 45.6962567581079d, 728.15643854615257d)]
    [InlineData(999, 41.3380610642585, 45.6962567581079, null)]
    public void CalculateDistanceTest(int fieldId, double lat, double lon, double? expectedResult)
    {
        // Arrange
        var fieldService = new FieldService(new FieldRepository(), new GeometryFactory());

        // Act
        var result = fieldService.CalculateDistance(fieldId, lat, lon);

        // Assert
        Assert.True(result == expectedResult);
    }

    [Theory]
    [InlineData(1, 41.3380610642585d, 45.6962567581079d)]
    [InlineData(1, 41.3346809239899d, 45.7074047669366d)]
    [InlineData(null, 41.3346809239899d, 45.7074047669367d)]
    [InlineData(null, 0, 0)]
    public void CheckPointInField(int? expectedFieldId, double lat, double lon)
    {
        // Arrange
        var fieldService = new FieldService(new FieldRepository(), new GeometryFactory());

        // Act
        var result = fieldService.FindFieldContainingPoint(lat, lon);

        // Assert
        if (expectedFieldId.HasValue)
        {
            Assert.NotNull(result);
            Assert.Equal(expectedFieldId.Value, result.Id);
        }
        else
        {
            Assert.Null(result);
        }
    }
}