using CPM_backend.Models;
using CPM_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPM_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly EpisodesService _episodesService;
    private readonly ProjectsService _projectsService;

    public ProjectsController(ProjectsService projectsService, EpisodesService episodesService)
    {
        _projectsService = projectsService;
        _episodesService = episodesService;
    }

    [HttpGet]
    [Authorize]
    public async Task<List<Project>> Get()
    {
        return await _projectsService.GetAsync();
    }

    [HttpGet("{id:length(24)}")]
    [Authorize]
    public async Task<ActionResult<Project>> Get(string id)
    {
        var project = await _projectsService.GetAsync(id);

        if (project is null)
            return NotFound();

        return project;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(Project newProject)
    {
        await _projectsService.CreateAsync(newProject);

        if (newProject.ProjectType == ProjectType.Movie)
            await _episodesService.CreateAsync(newProject.Id, new Episode());

        return CreatedAtAction(nameof(Get), new { id = newProject.Id }, newProject);
    }

    [HttpPut("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, ProjectUpdateDTO updatedProject)
    {
        var project = await _projectsService.GetAsync(id);

        if (project is null)
            return NotFound();

        await _projectsService.UpdateAsync(id, updatedProject);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var project = await _projectsService.GetAsync(id);

        if (project is null)
            return NotFound();

        await _projectsService.RemoveAsync(id);

        return NoContent();
    }
}