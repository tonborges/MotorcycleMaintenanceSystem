using System.Text.Json.Serialization;
using Domain.Motorcycles;

namespace Application.Features.Motorcycles.GetById;

public record GetMotorcycleByIdResponse
{
    [JsonPropertyName("identificador")]
    public string? Identifier { get; set; }
    [JsonPropertyName("ano")]
    public int? Year { get; set; }
    [JsonPropertyName("modelo")]
    public string? Model { get; set; }
    [JsonPropertyName("placa")]
    public string? Plate { get; set; }

    public static implicit operator GetMotorcycleByIdResponse?(Motorcycle? entity)
        => entity is null
            ? null
            : new GetMotorcycleByIdResponse
            {
                Identifier = entity.Identifier,
                Year = entity.Year,
                Model = entity.Model,
                Plate = entity.Plate,
            };
}