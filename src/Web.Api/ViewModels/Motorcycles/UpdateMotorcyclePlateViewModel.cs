using System.Text.Json.Serialization;
using Application.Features.Motorcycles.UpdatePlate;

namespace Web.Api.ViewModels.Motorcycles;

public class UpdateMotorcyclePlateViewModel
{
    [JsonPropertyName("placa")]
    public string? Plate { get; set; }
    
    public static explicit operator UpdateMotorcyclePlateCommand(UpdateMotorcyclePlateViewModel viewModel)
        => new()
        {
            Plate = viewModel.Plate
        };
}