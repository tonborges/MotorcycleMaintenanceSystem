using Application.Features.Rents.Create;
using Application.Features.Rents.GetById;
using Application.Features.Rents.UpdateReturn;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Results;
using Swashbuckle.AspNetCore.Annotations;
using Web.Api.Configurations;
using Web.Api.ViewModels.Rents;

namespace Web.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("locacao")]
public class RentController(ISender sender, IOptions<AppSettings> options) : Controller
{
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Search for rentals by Id")]
    [Produces(typeof(GetRentByIdResponse))]
    [ProducesResponseType(200, Type = typeof(GetRentByIdResponse))]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    [ProducesResponseType(404, Type = typeof(MessageResult))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string? id)
        => (await sender.Send(new GetRentByIdQuery(id)))
            .Match<IActionResult>
            (
                onSuccess: os =>
                {
                    if (os.Notifications?.Any() == true)
                        return NotFound(new MessageResult(os.Notifications?.FirstOrDefault()?.Message));

                    return Ok(os.Data);
                },
                onFailure: of => NotFound(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );

    [HttpPost]
    [SwaggerOperation(Summary = "Register a new rental")]
    [Produces(typeof(CreateRentViewModel))]
    [ProducesResponseType(201)]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    public async Task<IActionResult> CreateRentAsync([FromBody] CreateRentViewModel viewModel)
        => (await sender.Send((CreateRentCommand)viewModel))
            .Match<IActionResult>
            (
                onSuccess: os => Created($"{options.Value.BaseUrl}{os.Id}", os.Id),
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );

    [HttpPut("{id}/devolucao")]
    [SwaggerOperation(Summary = "Enter return date and calculate value")]
    [Produces(typeof(UpdateRentReturnViewModel))]
    [ProducesResponseType(200, Type = typeof(MessageResult))]
    [ProducesResponseType(400, Type = typeof(MessageResult))]
    public async Task<IActionResult> AlterRentAsync([FromRoute] string id, [FromBody] UpdateRentReturnViewModel viewModel)
        => (await sender.Send(((UpdateRentReturnCommand)viewModel).WithId(id)))
            .Match<IActionResult>
            (
                onSuccess: os => Ok(new MessageResult(os.Notifications?.FirstOrDefault()?.Message)),
                onFailure: of => BadRequest(new MessageResult(of.Notifications?.FirstOrDefault()?.Message))
            );
}