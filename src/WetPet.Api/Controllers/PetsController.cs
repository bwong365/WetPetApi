using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WetPet.AppCore.Services.Commands.AddPet;
using WetPet.AppCore.Services.Commands.ReleasePet;
using WetPet.AppCore.Services.Commands.UpdatePet;
using WetPet.AppCore.Services.Queries.GetPets;
using WetPet.AppCore.Services.Queries.GetPetReport;
using WetPet.Contracts.Pets;

namespace WetPet.Api.Controllers;

[Route("[controller]")]
public class PetsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PetsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet(Name = "getPets")]
    [ProducesResponseType(typeof(List<PetResponse>), 200)]
    public async Task<IActionResult> GetPetsAsync()
    {
        // TODO: Get sub from JWT
        var sub = "test-sub";
        var result = await _sender.Send(new GetPetsQuery { Sub = sub });
        return result.Match(
            pets => Ok(_mapper.Map<List<PetResponse>>(pets)),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}/report", Name = "getPetReport")]
    [ProducesResponseType(typeof(PetReportResponse), 200)]
    public async Task<IActionResult> GetPetReportAsync(Guid id, CancellationToken ct)
    {
        // TODO: Get sub from JWT
        var sub = "test-sub";
        var result = await _sender.Send(new GetPetReportQuery { Sub = sub, PetId = id }, ct);
        return result.Match(
            weather => Ok(_mapper.Map<PetReportResponse>(weather)),
            errors => Problem(errors)
        );
    }


    [HttpPost(Name = "addPet")]
    [ProducesResponseType(typeof(PetResponse), 201)]
    public async Task<IActionResult> AddPetAsync([FromBody] PetAdditionRequest request, CancellationToken ct)
    {
        // TODO: Get sub from JWT
        var sub = "test-sub";
        var command = _mapper.Map<AddPetCommand>((sub, request));
        var result = await _sender.Send(command, ct);
        return result.Match(
            pet => Created($"/pets/{pet.Id}", _mapper.Map<PetResponse>(pet)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{petId:guid}", Name = "updatePet")]
    [ProducesResponseType(typeof(PetResponse), 200)]
    public async Task<IActionResult> UpdatePetAsync([FromRoute] Guid petId, [FromBody] PetUpdateRequest request, CancellationToken ct)
    {
        // TODO: Get sub from JWT
        var sub = "test-sub";
        var command = _mapper.Map<UpdatePetCommand>((sub, petId, request));
        var result = await _sender.Send(command, ct);
        return result.Match(
            pet => Ok(_mapper.Map<PetResponse>(pet)),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{petId:guid}", Name = "releasePet")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> ReleasePetAsync([FromRoute] Guid petId, CancellationToken ct)
    {
        // TODO: Get sub from JWT
        var sub = "test-sub";
        var command = new ReleasePetCommand { PetId = petId, Sub = sub };
        var result = await _sender.Send(command, ct);
        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}