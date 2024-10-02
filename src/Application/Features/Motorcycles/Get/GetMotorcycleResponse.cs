using System.Text.Json.Serialization;
using Domain.Motorcycles;

namespace Application.Features.Motorcycles.Get;

public record GetMotorcycleResponse
{
    [JsonPropertyName("identificador")]
    public string? Identifier { get; set; }
    [JsonPropertyName("ano")]
    public int? Year { get; set; }
    [JsonPropertyName("modelo")]
    public string? Model { get; set; }
    [JsonPropertyName("placa")]
    public string? Plate { get; set; }

    public static explicit operator GetMotorcycleResponse?(Motorcycle? entity)
        => entity is null
            ? null
            : new GetMotorcycleResponse
            {
                Identifier = entity.Identifier,
                Year = entity.Year,
                Model = entity.Model,
                Plate = entity.Plate,
            };
}