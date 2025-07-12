using System.Text.Json.Serialization;

namespace Energomera.DTO;

public class FieldDto : FieldBasicDto
{
    [JsonPropertyOrder(3)]
    public double Size { get; set; }
    
    [JsonPropertyOrder(4)]
    public LocationsDto Locations { get; set; }
}