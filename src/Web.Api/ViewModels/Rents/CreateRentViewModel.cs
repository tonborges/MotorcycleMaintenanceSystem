using System.Text.Json.Serialization;
using Application.Features.Rents.Create;

namespace Web.Api.ViewModels.Rents;

public class CreateRentViewModel
{
    [JsonPropertyName("entregador_id")] public string? DeliveryPersonId { get; set; }
    [JsonPropertyName("moto_id")] public string? MotorcycleId { get; set; }
    [JsonPropertyName("data_inicio")] public DateTime? StartDate { get; set; }
    [JsonPropertyName("data_termino")] public DateTime? EndDate { get; set; }

    [JsonPropertyName("data_previsao_termino")]
    public DateTime? ForecastDateEnd { get; set; }

    [JsonPropertyName("plano")] public int? Plan { get; set; }

    public static explicit operator CreateRentCommand(CreateRentViewModel command)
        => new()
        {
            DeliveryPersonId = command.DeliveryPersonId,
            MotorcycleId = command.MotorcycleId,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            ForecastDateEnd = command.ForecastDateEnd,
            Plan = command.Plan,
        };
}