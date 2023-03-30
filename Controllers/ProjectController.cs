using CPMApi.Models;
using CPMApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectsService _ProjectsService;
    private readonly EpisodesService _EpisodesService;

    public ProjectsController(ProjectsService ProjectsService, EpisodesService EpisodesService)
    {
        _ProjectsService = ProjectsService;
        _EpisodesService = EpisodesService;
    }

    [HttpGet, Authorize]
    public async Task<List<Project>> Get() => await _ProjectsService.GetAsync();

    [HttpGet("{id:length(24)}"), Authorize]
    public async Task<ActionResult<Project>> Get(string id)
    {
        var Project = await _ProjectsService.GetAsync(id);

        if (Project is null)
        {
            return NotFound();
        }

        return Project;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Post(Project newProject)
    {
        await _ProjectsService.CreateAsync(newProject);

        if (newProject.isFilm)
        {
            Episode placeholderEpisode = new Episode();
            await _EpisodesService.CreateAsync(newProject.Id!, placeholderEpisode);
        }

        return CreatedAtAction(nameof(Get), new { id = newProject.Id }, newProject);
    }

    [HttpPut("{id:length(24)}"), Authorize]
    public async Task<IActionResult> Update(string id, ProjectUpdateDTO updatedProject)
    {
        var project = await _ProjectsService.GetAsync(id);

        if (project is null)
        {
            return NotFound();
        }

        await _ProjectsService.UpdateAsync(id, updatedProject);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}"), Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var Project = await _ProjectsService.GetAsync(id);

        if (Project is null)
        {
            return NotFound();
        }

        await _ProjectsService.RemoveAsync(id);

        return NoContent();
    }
}
