using Application.Abstractions.Messaging;
using Domain.Rents;

namespace Application.Features.Rents.Create;

public class CreateRentCommand : ICommand
{
    public string? DeliveryPersonId { get; set; }
    public string? MotorcycleId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ForecastDateEnd { get; set; }
    public int? Plan { get; set; }

    public static explicit operator Rent(CreateRentCommand command)
         => new Rent()
                .SetDeliveryPersonId(command.DeliveryPersonId)
                .SetMotorcycleId(command.MotorcycleId)
                .SetStartDate(command.StartDate)
                .SetEndDate(command.EndDate)
                .SetForecastDateEnd(command.ForecastDateEnd)
                .SetPlan(command.Plan)
                ;
}
