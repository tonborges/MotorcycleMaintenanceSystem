using Application.Features.DeliveryPeople.Create;
using Application.Features.DeliveryPeople.UpdateDriversLicense;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using Swashbuckle.AspNetCore.Annotations;
using Web.Api.ViewModels.DeliveryPeople;

namespace Web.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("entregadores")]
public class DeliveryPersonController(ISender sender) : Controller
{
    [HttpPost]
    [SwaggerOperation(Summary = "Register delivery person")]
    [Produces(typeof(CreateDeliveryPersonViewModel))]
    [ProducesResponseType(201)]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    public async Task<IActionResult> CreateDeliveryPersonAsync([FromBody] CreateDeliveryPersonViewModel viewModel)
        => (await sender.Send((CreateDeliveryPersonCommand)viewModel))
            .Match<IActionResult>
            (
                onSuccess: os => Created("", os.Id),
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );

    [HttpPost("{id}/cnh")]
    [SwaggerOperation(Summary = "Send photo driver's license")]
    [Produces(typeof(UpdateDeliveryPersonDriversLicenseViewModel))]
    [ProducesResponseType(201)]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    public async Task<IActionResult> UpdateDriversLicenseDeliveryPersonAsync([FromRoute] string? id, [FromBody] UpdateDeliveryPersonDriversLicenseViewModel viewModel)
        => (await sender.Send(((UpdateDeliveryPersonDriversLicenseCommand)viewModel).WithIdentifier(id)))
            .Match<IActionResult>
            (
                onSuccess: os => Created("", os.Id),
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );
}