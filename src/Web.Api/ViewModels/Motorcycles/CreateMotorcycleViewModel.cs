using System.Text.Json.Serialization;
using Application.Features.Motorcycles.Create;

namespace Web.Api.ViewModels.Motorcycles;

public sealed class CreateMotorcycleViewModel
{
    [JsonPropertyName("identificador")] public string? Identifier { get; set; }
    [JsonPropertyName("ano")] public int? Year { get; set; }
    [JsonPropertyName("modelo")] public string? Model { get; set; }
    [JsonPropertyName("placa")] public string? Plate { get; set; }
    
    public static explicit operator CreateMotorcycleCommand(CreateMotorcycleViewModel viewModel)
        => new()
        {
            Identifier = viewModel.Identifier,
            Year = viewModel.Year,
            Model = viewModel.Model,
            Plate = viewModel.Plate
        };
}