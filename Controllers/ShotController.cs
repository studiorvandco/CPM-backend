using CPMApi.Models;
using CPMApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShotsController : ControllerBase
{
    private readonly ShotsService _ShotsService;

    public ShotsController(ShotsService ShotsService) =>
        _ShotsService = ShotsService;

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}"), Authorize]
    public async Task<List<Shot>> Get(string idProject, string idEpisode, string idSequence) =>
        await _ShotsService.GetAsync(idProject, idEpisode, idSequence);

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}/{idShot:length(24)}"), Authorize]
    public async Task<ActionResult<Shot>> Get(string idProject, string idEpisode, string idSequence, string idShot)
    {
        var Shot = await _ShotsService.GetAsync(idProject, idEpisode, idSequence, idShot);

        if (Shot is null)
        {
            return NotFound();
        }

        return Shot;
    }

    [HttpPost("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}"), Authorize]
    public async Task<IActionResult> Post(string idProject, string idEpisode, string idSequence, Shot newShot)
    {
        await _ShotsService.CreateAsync(idProject, idEpisode, idSequence, newShot);

        return CreatedAtAction(nameof(Get), new { idProject, idEpisode, idSequence, idShot = newShot.Id }, newShot);
    }

    [HttpPut("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}/{idShot:length(24)}"), Authorize]
    public async Task<IActionResult> Update(string idProject, string idEpisode, string idSequence, string idShot, Shot updatedShot)
    {
        var Shot = await _ShotsService.GetAsync(idProject, idEpisode, idSequence, idShot);

        if (Shot is null)
        {
            return NotFound();
        }

        await _ShotsService.UpdateAsync(idProject, idEpisode, idSequence, idShot, updatedShot);

        return NoContent();
    }

    [HttpDelete("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}/{idShot:length(24)}"), Authorize]
    public async Task<IActionResult> Delete(string idProject, string idEpisode, string idSequence, string idShot)
    {
        var Shot = await _ShotsService.GetAsync(idProject, idEpisode, idSequence, idShot);

        if (Shot is null)
        {
            return NotFound();
        }

        await _ShotsService.RemoveAsync(idProject, idEpisode, idSequence, idShot);

        return NoContent();
    }
}