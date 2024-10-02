using Application.Features.Motorcycles.Create;
using Application.Features.Motorcycles.Delete;
using Application.Features.Motorcycles.Get;
using Application.Features.Motorcycles.GetById;
using Application.Features.Motorcycles.UpdatePlate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using Swashbuckle.AspNetCore.Annotations;
using Web.Api.ViewModels.Motorcycles;

namespace Web.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("motos")]
public class MotorcycleController(ISender sender) : Controller
{
    [HttpGet]
    [SwaggerOperation(Summary = "Search existing motorcycles")]
    [Produces(typeof(GetMotorcycleResponse[]))]
    public async Task<IActionResult> GetAsync([FromQuery] string? placa)
        => (await sender.Send(new GetMotorcycleQuery(placa)))
            .Match<IActionResult>
            (
                onSuccess: os => Ok(os.Data),
                onFailure: of => Ok(default(GetMotorcycleResponse[]))
            );

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Search for existing motorcycles by Id")]
    [Produces(typeof(GetMotorcycleResponse))]
    [ProducesResponseType(200, Type = typeof(GetMotorcycleResponse))]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    [ProducesResponseType(404, Type = typeof(MessageResult))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string? id)
        => (await sender.Send(new GetMotorcycleByIdQuery(id)))
            .Match<IActionResult>
            (
                onSuccess: os =>
                {
                    if (os.Notifications?.Any() == true)
                        return NotFound(new MessageResult(os.Notifications?.FirstOrDefault()?.Message));

                    return Ok(os.Data);
                },
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );

    [HttpPost]
    [SwaggerOperation(Summary = "Register a new motorcycle")]
    [Produces(typeof(CreateMotorcycleViewModel))]
    [ProducesResponseType(201)]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    public async Task<IActionResult> CreateMotorcycleAsync([FromBody] CreateMotorcycleViewModel viewModel)
        => (await sender.Send((CreateMotorcycleCommand)viewModel))
            .Match<IActionResult>
            (
                onSuccess: os => Created("", os.Id),
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );

    [HttpPut("{id}/placa")]
    [SwaggerOperation(Summary = "Modify a motorcycle's plate")]
    [Produces(typeof(UpdateMotorcyclePlateViewModel))]
    [ProducesResponseType(200, Type = typeof(MessageResult))]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    public async Task<IActionResult> AlterMotorcycleAsync([FromRoute] string? id, [FromBody] UpdateMotorcyclePlateViewModel viewModel)
        => (await sender.Send(((UpdateMotorcyclePlateCommand)viewModel).WithIdentifier(id)))
            .Match<IActionResult>
            (
                onSuccess: os => Ok(new MessageResult(os.Notifications?.FirstOrDefault()?.Message)),
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remove a motorcycle")]
    [Produces(typeof(DeleteMotorcycleCommand))]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    public async Task<IActionResult> DeleteMotorcycleAsync([FromRoute] string id)
        => (await sender.Send(new DeleteMotorcycleCommand(id)))
            .Match<IActionResult>
            (
                onSuccess: Ok,
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );
}