using System.Text.Json.Serialization;
using Domain.Rents;

namespace Application.Features.Rents.GetById;

public class GetRentByIdResponse
{
    [JsonPropertyName("identificador")] 
    public Guid Id { get; set; }
    [JsonPropertyName("valor_diaria")] 
    public decimal? DailyPrice { get; set; }
    [JsonPropertyName("entregador_id")] 
    public string? DeliveryPersonId { get; set; }
    [JsonPropertyName("moto_id")] 
    public string? MotorcycleId { get; set; }
    [JsonPropertyName("data_inicio")]
    public DateTime? StartDate { get; set; }
    [JsonPropertyName("data_termino")] 
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("data_previsao_termino")]
    public DateTime? ForecastDateEnd { get; set; }

    [JsonPropertyName("data_devolucao")] 
    public DateTime? ReturnDate { get; set; }

    public static implicit operator GetRentByIdResponse(Rent entity)
    {
        return new GetRentByIdResponse
        {
            Id = entity.Id,
            DailyPrice = new Plan().GetPlanById(entity.Plan)?.PricePerDay,
            DeliveryPersonId = entity.DeliveryPersonId,
            MotorcycleId = entity.MotorcycleId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            ForecastDateEnd = entity.ForecastDateEnd,
            ReturnDate = entity.ReturnDate
        };
    }
}