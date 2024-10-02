using Application.Features.Rents.UpdateReturn;

namespace Web.Api.ViewModels.Rents;

public class UpdateRentReturnViewModel
{
    public DateTime? EndDate { get; set; }
    
    public static explicit operator UpdateRentReturnCommand(UpdateRentReturnViewModel viewModel)
        => new()
        {
            EndDate = viewModel.EndDate,
        };
}