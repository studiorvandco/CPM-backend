using CPM_backend.Models;
using CPM_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPM_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShotsController : ControllerBase
{
    private readonly ShotsService _shotsService;

    public ShotsController(ShotsService shotsService)
    {
        _shotsService = shotsService;
    }

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}")]
    [Authorize]
    public async Task<List<Shot>> Get(string idProject, string idEpisode, string idSequence)
    {
        return await _shotsService.GetAsync(idProject, idEpisode, idSequence);
    }

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}/{idShot:length(24)}")]
    [Authorize]
    public async Task<ActionResult<Shot>> Get(string idProject, string idEpisode, string idSequence, string idShot)
    {
        var shot = await _shotsService.GetAsync(idProject, idEpisode, idSequence, idShot);

        if (shot is null) return NotFound();

        return shot;
    }

    [HttpPost("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Post(string idProject, string idEpisode, string idSequence, Shot newShot)
    {
        await _shotsService.CreateAsync(idProject, idEpisode, idSequence, newShot);

        return CreatedAtAction(nameof(Get), new { idProject, idEpisode, idSequence, idShot = newShot.Id }, newShot);
    }

    [HttpPut("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}/{idShot:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(string idProject, string idEpisode, string idSequence, string idShot,
        ShotUpdateDTO updatedShot)
    {
        var shot = await _shotsService.GetAsync(idProject, idEpisode, idSequence, idShot);

        if (shot is null) return NotFound();

        await _shotsService.UpdateAsync(idProject, idEpisode, idSequence, idShot, updatedShot);

        return NoContent();
    }

    [HttpDelete("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}/{idShot:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string idProject, string idEpisode, string idSequence, string idShot)
    {
        var shot = await _shotsService.GetAsync(idProject, idEpisode, idSequence, idShot);

        if (shot is null) return NotFound();

        await _shotsService.RemoveAsync(idProject, idEpisode, idSequence, shot);

        return NoContent();
    }
}