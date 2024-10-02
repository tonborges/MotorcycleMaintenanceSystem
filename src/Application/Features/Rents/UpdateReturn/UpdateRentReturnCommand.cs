using Application.Abstractions.Messaging;

namespace Application.Features.Rents.UpdateReturn;

public class UpdateRentReturnCommand : ICommand
{
    public string? Id { get; set; }
    public DateTime? EndDate { get; set; }

    public UpdateRentReturnCommand WithId(string? id)
    {
        Id = id;
        return this;
    }
}