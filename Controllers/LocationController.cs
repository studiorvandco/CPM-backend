using CPM_backend.Models;
using CPM_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPM_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly LocationsService _locationsService;

    public LocationsController(LocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    [HttpGet]
    [Authorize]
    public async Task<List<Location>> Get()
    {
        return await _locationsService.GetAsync();
    }

    [HttpGet("{id:length(24)}")]
    [Authorize]
    public async Task<ActionResult<Location>> Get(string id)
    {
        var location = await _locationsService.GetAsync(id);

        if (location is null) return NotFound();

        return location;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(Location newLocation)
    {
        await _locationsService.CreateAsync(newLocation);

        return CreatedAtAction(nameof(Get), new { id = newLocation.Id }, newLocation);
    }

    [HttpPut("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, Location updatedLocation)
    {
        var location = await _locationsService.GetAsync(id);

        if (location is null) return NotFound();

        await _locationsService.UpdateAsync(id, updatedLocation);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var location = await _locationsService.GetAsync(id);

        if (location is null) return NotFound();

        await _locationsService.RemoveAsync(id);

        return NoContent();
    }
}