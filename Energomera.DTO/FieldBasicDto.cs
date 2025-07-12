using System.Text.Json.Serialization;

namespace Energomera.DTO;

public class FieldBasicDto
{
    [JsonPropertyOrder(1)]
    public int Id { get; set; }

    [JsonPropertyOrder(2)]
    public string Name { get; set; }
}
