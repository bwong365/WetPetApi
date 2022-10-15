using MediatR;
using Microsoft.AspNetCore.Mvc;
using WetPet.AppCore.Services.Commands.CreateOwner;

namespace WetPet.Api.Controllers;

[Route("[controller]")]
public class OwnersController : ApiController
{
    private readonly ISender _sender;

    public OwnersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreateOwnerAsync(CancellationToken ct)
    {
        // TODO: Get sub from JWT
        var sub = "test-sub";
        var command = new CreateOwnerCommand { Sub = sub };
        var result = await _sender.Send(command, ct);
        return result.Match(
            success => Created("/user", null),
            errors => Problem(errors)
        );
    }
}