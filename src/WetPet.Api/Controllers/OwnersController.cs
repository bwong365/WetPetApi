using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WetPet.AppCore.Services.Commands.CreateOwner;
using WetPet.Contracts.Owners;

namespace WetPet.Api.Controllers;

[Route("[controller]")]
public class OwnersController : ApiController
{
    private readonly ISender _sender;

    public OwnersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(Name = "createOwner")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreateOwnerAsync(CancellationToken ct)
    {
        var command = new CreateOwnerCommand { Sub = Sub };
        var result = await _sender.Send(command, ct);
        return result.Match(
            success => Created("/owner", null),
            errors => Problem(errors)
        );
    }

    [HttpPost("webhook", Name = "createOwnerWebhook")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType(201)]
    [AllowAnonymous]
    [DisableCors]
    public async Task<IActionResult> CreateOwnerWebhookAsync([FromBody] OwnerWebhookCreationRequest request, CancellationToken ct)
    {
        var command = new CreateOwnerCommand { Sub = request.Sub };
        var result = await _sender.Send(command, ct);
        return result.Match(
            success => Created("/owner", null),
            errors => Problem(errors)
        );
    }
}