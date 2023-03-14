using CPMApi.Models;
using CPMApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EpisodesController : ControllerBase
{
    private readonly EpisodesService _EpisodesService;

    public EpisodesController(EpisodesService EpisodesService) =>
        _EpisodesService = EpisodesService;

    [HttpGet("{idProject:length(24)}"), Authorize]
    public async Task<List<Episode>> Get(string idProject) =>
        await _EpisodesService.GetAsync(idProject);

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}"), Authorize]
    public async Task<ActionResult<Episode>> Get(string idProject, string idEpisode)
    {
        var Episode = await _EpisodesService.GetAsync(idProject, idEpisode);

        if (Episode is null)
        {
            return NotFound();
        }

        Episode.Sequences = new List<Sequence>();

        return Episode;
    }

    [HttpPost("{idProject:length(24)}"), Authorize]
    public async Task<IActionResult> Post(string idProject, Episode episode)
    {
        if (episode.Id == null)
            episode.setId();

        await _EpisodesService.CreateAsync(idProject, episode);

        return CreatedAtAction(nameof(Get), new { idProject, idEpisode = episode.Id }, episode);
    }

    [HttpPut("{idProject:length(24)}/{idEpisode:length(24)}"), Authorize]
    public async Task<IActionResult> Update(string idProject, string idEpisode, Episode uptadeEpisode)
    {
        var episode = await _EpisodesService.GetAsync(idProject, idEpisode);

        if (episode is null)
        {
            return NotFound();
        }

        uptadeEpisode.Id = episode.Id;

        await _EpisodesService.UpdateAsync(idProject, idEpisode, uptadeEpisode);

        return NoContent();
    }

    [HttpDelete("{idProject:length(24)}/{idEpisode:length(24)}"), Authorize]
    public async Task<IActionResult> Delete(string idProject, string idEpisode)
    {
        var episode = await _EpisodesService.GetAsync(idProject, idEpisode);

        if (episode is null)
        {
            return NotFound();
        }

        await _EpisodesService.RemoveAsync(idProject, idEpisode);

        return NoContent();
    }
}