using System.Text.Json.Serialization;
using Application.Features.DeliveryPeople.Create;

namespace Web.Api.ViewModels.DeliveryPeople;

public class CreateDeliveryPersonViewModel
{
    [JsonPropertyName("identificador")]
    public string? Identifier { get; set; }
    [JsonPropertyName("nome")]
    public string? Name { get; set; }
    [JsonPropertyName("cnpj")]
    public string? Ein { get; set; }
    [JsonPropertyName("data_nascimento")]
    public DateTime? DateOfBirth { get; set; }
    [JsonPropertyName("numero_cnh")]
    public string? DriversLicenseNumber { get; set; }
    [JsonPropertyName("tipo_cnh")]
    public string? DriversLicenseType { get; set; }
    [JsonPropertyName("imagem_cnh")]
    public string? DriversLicenseImage { get; set; }
    

    public static explicit operator CreateDeliveryPersonCommand(CreateDeliveryPersonViewModel viewModel)
        => new()
        {
            Identifier = viewModel.Identifier,
            Name = viewModel.Name,
            Ein = viewModel.Ein,
            DateOfBirth = viewModel.DateOfBirth,
            DriversLicenseNumber = viewModel.DriversLicenseNumber,
            DriversLicenseType = viewModel.DriversLicenseType,
            DriversLicenseImage = viewModel.DriversLicenseImage,
        };
}