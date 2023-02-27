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

    public ProjectsController(ProjectsService ProjectsService) =>
        _ProjectsService = ProjectsService;

    [HttpGet, Authorize]
    public async Task<List<Project>> Get() =>
        await _ProjectsService.GetAsync();

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

        return CreatedAtAction(nameof(Get), new { id = newProject.Id }, newProject);
    }

    [HttpPut("{id:length(24)}"), Authorize]
    public async Task<IActionResult> Update(string id, Project updatedProject)
    {
        var project = await _ProjectsService.GetAsync(id);

        if (project is null)
        {
            return NotFound();
        } 

        Project newProject = project.cloneProject();

        if (updatedProject.Title != null)
            newProject.Title = updatedProject.Title;

        if (updatedProject.Description != null)
            newProject.Description = updatedProject.Description;

        if (updatedProject.BeginDate != null)
            newProject.BeginDate = updatedProject.BeginDate;
        
        if (updatedProject.EndDate != null)
            newProject.EndDate = updatedProject.EndDate;
        
        // WARNING : We always replace new value here
        newProject.ShotsTotal = updatedProject.ShotsTotal;
        newProject.ShotsCompleted = updatedProject.ShotsCompleted;

        if (updatedProject.isFilm != null)
            newProject.isFilm = updatedProject.isFilm;

        if (updatedProject.isSerie != null)
            newProject.isSerie = updatedProject.isSerie;

        if (updatedProject.Episodes != null)
            newProject.Episodes = updatedProject.Episodes;

        await _ProjectsService.UpdateAsync(id, newProject);

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
