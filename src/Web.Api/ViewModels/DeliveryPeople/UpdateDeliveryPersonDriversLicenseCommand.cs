using System.Text.Json.Serialization;
using Application.Features.DeliveryPeople.UpdateDriversLicense;

namespace Web.Api.ViewModels.DeliveryPeople;

public class UpdateDeliveryPersonDriversLicenseViewModel
{
    [JsonPropertyName("imagem_cnh")] public string? DriversLicenseImage { get; set; }

    public static explicit operator UpdateDeliveryPersonDriversLicenseCommand(UpdateDeliveryPersonDriversLicenseViewModel viewModel)
        => new()
        {
            DriversLicenseImage = viewModel.DriversLicenseImage,
        };
}