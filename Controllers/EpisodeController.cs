using CPM_backend.Models;
using CPM_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPM_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EpisodesController : ControllerBase
{
    private readonly EpisodesService _episodesService;

    public EpisodesController(EpisodesService episodesService)
    {
        _episodesService = episodesService;
    }

    [HttpGet("{idProject:length(24)}")]
    [Authorize]
    public async Task<List<Episode>> Get(string idProject)
    {
        return await _episodesService.GetAsync(idProject);
    }

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}")]
    [Authorize]
    public async Task<ActionResult<Episode>> Get(string idProject, string idEpisode)
    {
        var episode = await _episodesService.GetAsync(idProject, idEpisode);

        if (episode is null)
            return NotFound();

        return episode;
    }

    [HttpPost("{idProject:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Post(string idProject, Episode episode)
    {
        await _episodesService.CreateAsync(idProject, episode);

        return CreatedAtAction(nameof(Get), new { idProject, idEpisode = episode.Id }, episode);
    }

    [HttpPut("{idProject:length(24)}/{idEpisode:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(
        string idProject,
        string idEpisode,
        EpisodeUpdateDTO updatedEpisode
    )
    {
        var episode = await _episodesService.GetAsync(idProject, idEpisode);

        if (episode is null)
            return NotFound();

        await _episodesService.UpdateAsync(idProject, idEpisode, updatedEpisode);

        return NoContent();
    }

    [HttpDelete("{idProject:length(24)}/{idEpisode:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string idProject, string idEpisode)
    {
        var episode = await _episodesService.GetAsync(idProject, idEpisode);

        if (episode is null)
            return NotFound();

        await _episodesService.RemoveAsync(idProject, episode);

        return NoContent();
    }
}
